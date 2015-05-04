using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;
using System.Globalization;

namespace Perceptive.IHE.AuditTrail
{
    [Serializable]
    public abstract class XmlSectionBase
    {
        # region Properties

        /// <summary>
        /// Gets or sets the actor.
        /// </summary>
        /// <value>The actor.</value>
        [XmlIgnore]
        public MessageType Actor { get; set; }

        /// <summary>
        /// Gets or sets the type of the section.
        /// </summary>
        /// <value>The type of the section.</value>
        [XmlIgnore]
        public SectionType SectionType { get; set; }

        # endregion

        # region Methods

        /// <summary>
        /// Populates the default data.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void PopulateDefaultData(string propertyName)
        {
            foreach (var property in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (property.PropertyType.BaseType.IsEquivalentTo(typeof(XmlSectionBase)))
                {
                    object obj = property.GetValue(this, null);
                    if (obj != null)
                    {
                        PropertyInfo actorPropertyInfo = obj.GetType().GetProperty("Actor");
                        actorPropertyInfo.SetValue(obj, Actor, null);
                        if (SectionType != IHE.AuditTrail.SectionType.None)
                        {
                            PropertyInfo sectionPropertyInfo = obj.GetType().GetProperty("SectionType");
                            sectionPropertyInfo.SetValue(obj, SectionType, null);
                        }

                        MethodInfo methodInfo = obj.GetType().
                            GetMethod("PopulateDefaultData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        methodInfo.Invoke(obj, new object[] { property.Name });
                    }
                }
                else if (property.PropertyType.IsGenericType)
                {
                    object obj = property.GetValue(this, null);

                    if (obj != null && obj.GetType().GetGenericArguments()[0].BaseType.IsEquivalentTo(typeof(XmlSectionBase)))
                    {
                        foreach (var item in obj as IEnumerable<XmlSectionBase>)
                        {
                            item.Actor = Actor;
                            if (SectionType != IHE.AuditTrail.SectionType.None)
                                item.SectionType = SectionType;

                            item.PopulateDefaultData(property.Name);
                        }
                    }
                }
            }
        }

        # endregion
    }
}
