using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Perceptive.IHE.AuditTrail;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Perceptive.ARR.HelperLibrary
{
    [Serializable]
    public class SyslogMessage: MessageBase
    {
        public Header Header { get; private set; }

        public string StructuredData { get; private set; }

        public string Message { get; private set; }

        public AuditMessage AuditMessage { get; private set; }

        public AuditMessageDicom AuditMessageDicom { get; private set; }

        public bool? IsDicomFormat { get; private set; }

        public SyslogMessage(string input): base(input)
        {
            try
            {
                string[] splitData = Data.Split(Constants.Separator.ToCharArray());

                if (splitData.Length < Constants.MinimumSectionCount)
                    return;
                else
                {
                    string[] headerInputs = new string[Constants.HeaderPartCount];
                    for (int i = 0; i < Constants.HeaderPartCount; i++)
                        headerInputs[i] = splitData[i];

                    Header = new Header(headerInputs);
                    if (!Header.IsValid)
                        return;

                    StructuredData = splitData[Constants.StructuredDataPosition - 1];

                    int indexesToIgnore = Constants.StructuredDataPosition - 1;

                    if (StructuredData.Equals(Constants.NilValue))
                    {
                        //Do not put valid yet.
                        //IsValid = true;
                    }
                    else if (StructuredData.StartsWith("["))
                    {
                        StringBuilder builder = new StringBuilder();
                        int j;
                        for (j = Constants.StructuredDataPosition - 1; j < splitData.Length; j++)
                        {
                            builder.Append(splitData[j]);
                            if (splitData[j].EndsWith("]"))
                                break;
                        }

                        if (j == splitData.Length)
                        {
                            IsValid = false;
                            StructuredData = string.Empty;
                            return;
                        }
                        else
                        {
                            indexesToIgnore = j;
                            StructuredData = builder.ToString();
                        }
                    }
                    else
                    {
                        StructuredData = string.Empty;
                        IsValid = false;
                        return;
                    }

                    Message = Helper.JoinString(splitData, indexesToIgnore);

                    //Exclude BOM symbol from the actual message
                    int xmlStartIndex = Message.IndexOf('<');
                    if (xmlStartIndex > 0)
                        Message = Message.Substring(xmlStartIndex);

                    try
                    {
                        //Try Decoding with RFC 3881 Schema
                        AuditMessage = AuditTrail.Deserialize<AuditMessage>(Message);
                        if (string.IsNullOrEmpty(AuditMessage.Event.EventId.Code))
                        {
                            //Might be Dicom format
                            AuditMessageDicom = AuditTrail.Deserialize<AuditMessageDicom>(Message);
                            IsDicomFormat = true;
                        }
                        else
                            IsDicomFormat = false;
                        IsValid = true;
                    }
                    catch (Exception ex)
                    {
                        Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                        Helper.LogMessage("Data received is: " + Message, Constants.LogCategoryName_Service);
                        IsValid = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
            }
        }
    }

    public class Header: MessageBase
    {
        public string Pri { get; private set; }
        public int? Version { get; private set; }
        public string Timestamp { get; private set; }
        public string HostName { get; private set; }
        public string AppName { get; private set; }
        public string ProcId { get; private set; }
        public string MsgId { get; private set; }

        public Header(string[] inputs): base(Helper.JoinString(inputs))
        {
            int priEndIndex = inputs[0].IndexOf(Constants.PriEnd);
            int tempValue;
            if (inputs[0].StartsWith(Constants.PriStart) && priEndIndex != -1)
            {
                Pri = inputs[0].Substring(0, priEndIndex + 1);
                
                if(int.TryParse(inputs[0].Substring(1, priEndIndex - 1), out tempValue) && tempValue >= 0 && tempValue <= 191)
                {
                    if(Pri.Length == inputs[0].Length)
                    {
                        // do nothing, its valid.
                    }
                    else
                    {
                        string tempVersion = inputs[0].Substring(priEndIndex + 1);
                        if(tempVersion.Length <= 2 && int.TryParse(tempVersion, out tempValue))
                            Version = tempValue;
                        else
                            return;
                    }

                    Timestamp = inputs[1];
                    HostName = inputs[2];
                    AppName = inputs[3];
                    ProcId = inputs[4];
                    MsgId = inputs[5];

                    if ((AppName.Equals(Constants.NilValue) || AppName.Length <= 48) &&
                        (HostName.Equals(Constants.NilValue) || HostName.Length <= 255) &&
                        (IsTimestampValid()) &&
                        (ProcId.Equals(Constants.NilValue) || ProcId.Length <= 128) &&
                        (MsgId.Equals(Constants.NilValue) || MsgId.Length <= 32))
                    {
                        IsValid = true;
                    }
                }
            }
        }

        private bool IsTimestampValid()
        {
            if (Timestamp.Equals(Constants.NilValue))
                return true;
            else
            {
                if(Timestamp.Contains("T"))
                {
                    try
                    {
                        int Tindex = Timestamp.IndexOf("T");
                        string fullDate = Timestamp.Substring(0, Tindex);
                        string fullTime = Timestamp.Substring(Tindex + 1, Timestamp.Length - Tindex - 1);
                        string[] dateSplit = fullDate.Split("-".ToCharArray());
                        int year, month, day;
                        if (dateSplit != null && dateSplit.Length == 3)
                        {
                            //Check if date is valid.
                            if (dateSplit[0].Length == 4 && dateSplit[1].Length == 2 && dateSplit[2].Length == 2 &&
                                int.TryParse(dateSplit[0], out year) && int.TryParse(dateSplit[1], out month) && int.TryParse(dateSplit[2], out day) &&
                                year > 0 && month > 0 && month <= 12 && day > 0 && day <= 31)
                            {
                                //Check if time is valid                                
                                int offsetStartIndex = fullDate.IndexOfAny(new char[] { 'Z', '+', '-' });
                                if (offsetStartIndex == -1)
                                    throw new InvalidCastException();

                                //TO DO : May try and check if the time string provided is valid
                                //char timeOffsetCharacter = fullDate[offsetStartIndex];

                                

                                

                                for (int i = 0; i < fullTime.Substring(0, offsetStartIndex).ToCharArray().Length; i++)
                                {
                                    switch (i)
                                    {
                                        case 2:
                                        case 5:
                                            if (fullTime.ElementAt(i) != ':')
                                                throw new InvalidCastException();
                                            break;
                                        case 0:
                                        case 1:
                                        case 3:
                                        case 4:
                                        case 6:
                                        case 7:
                                            if (!Char.IsDigit(fullTime.ElementAt(i)))
                                                throw new InvalidCastException();
                                            break;
                                        default:
                                            if (!(Char.IsDigit(fullTime.ElementAt(i)) || fullTime.ElementAt(i) == '.'))
                                                throw new InvalidCastException();
                                            break;
                                    }
                                }
                            }
                        }
                        return true;
                    }
                    catch (InvalidCastException)
                    {
                        // Do not throw anything to the higher level. Thrown only to return false at the end
                    }
                    catch (Exception ex)
                    {
                        Helper.LogMessage(ex.ToString(), Constants.LogCategoryName_Service);
                        return false;
                    }
                }
            }

            return false;
        }
    }

    [Serializable]
    public class MessageBase
    {
        public string Data { get; private set; }
        public bool IsValid { get; protected set; }

        public MessageBase(string input)
        {
            Data = input;            
        }
    }
}
