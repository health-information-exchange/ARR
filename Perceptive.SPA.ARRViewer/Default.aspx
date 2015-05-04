<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Perceptive.SPA.ARRViewer.Default" %>

<!DOCTYPE html>
<html>
	<head>	
		<title>Audit Record Repository Manager</title>
        <link rel="shortcut icon" type="image/ico" href="images/favicon.ico"/>
		<meta name="description" content="Welcome to my basic template.">
		<link rel="Stylesheet" href="stylesheets/Style.css" />
        <link rel="stylesheet" href="stylesheets/jquery-ui.css" />
        <link rel="stylesheet" href="stylesheets/jquery.datetimepicker.css" />
        <link rel="stylesheet" media="all" type="text/css" href="http://code.jquery.com/ui/1.10.4/themes/smoothness/jquery-ui.css" />
        <link rel="stylesheet" href="stylesheets/KoGrid.css" />
		<script src="scripts/knockout-3.1.0.js"></script>
		<script src="scripts/jquery-1.11.1.min.js"></script>
        <script src="scripts/jquery-ui-1.10.4.min.js"></script>
        <script src="scripts/jquery-ui-timepicker-addon.js"></script>
		<script src="scripts/jquery-ui-sliderAccess.js"></script>
		<script src="scripts/json2.js"></script>
		<script src="scripts/Sammy.js"></script>
        <script src="scripts/koGrid-2.1.1.js"></script>
		<script src="scripts/ViewModel.js"></script>
	</head>
	<body onload="LoadDefaultPage()">		
		<header class="header">   
            <img style="float:left; vertical-align:middle; display:inline-block" src="images/perceptivelogo.png" alt="Perceptive" />         
            <h3 style=" float:left; vertical-align:middle">&nbsp;Audit Record Repository Manager</h3>
            <span style="float:right">&nbsp;</span>
            <h4 style="float:right;" id="txtWelcome" runat="server"></h4>
            <h4 style="float:right;" >Welcome,&nbsp;</h4>
		</header>
		<div class="container">
			<nav class="nav">
				<ul class="views" data-bind="foreach: menu">
					<li data-bind="text: Name, css: { selected: Name == $root.chosenViewId() }, click: $root.goToView"></li>
				</ul>
			</nav>
			<div class="content">			
				<div class="innerContent" data-bind="visible: $root.showRecordData">
                    <div data-bind="with: searchFilter, visible: $root.showSearchPane" class="innerSubContentWithoutHeight">
                        <p>
                            <label>Maximum number of logs to display: <select data-bind="options: $root.NoOfItems, value: $root.logsToDisplay" ></select></label>
                            <button data-bind="click: $root.search">Search</button>
                            <button data-bind="click: $root.reset">Reset</button>
                            <label class="checkBoxLabel"><input class="checkBox" type="checkbox" data-bind="checked: $root.advancedSearch" />Advanced search</label>
                        </p>
                        <table class="table">
                            <tr>                                
                                <td><label>Source IP</label><br /><input type="text" data-bind="value: IPAddress" /></td>                                                                
                                <td><label>Logged from</label><br /><input type="text" data-bind="datetimepicker: $root.logged1" id="dtp1" /></td>                                
                                <td><label>Logged till</label><br /><input type="text" data-bind="datetimepicker: $root.logged2" id="dtp2" /></td>                                
                                <td><label>Host Name</label><br /><input type="text" data-bind="value: HostName" /></td>
                                <td><label>App Name</label><br /><input type="text" data-bind="value: AppName" /></td>
                                <td><label>Valid Logs: </label><br /><select data-bind="options: $root.validity, value: IsValid" ></select></td>                                
                            </tr>
                            <tr data-bind="visible: $root.advancedSearch">
                                <td><label>Participant Object Id</label><br /><input type="text" data-bind="value: ParticipantObjectId" /></td>
                                <td><label>Sent after</label><br /><input type="text" data-bind="datetimepicker: $root.sent1" id="dtp3" /></td>                                
                                <td><label>Sent before</label><br /><input type="text" data-bind="datetimepicker: $root.sent2" id="dtp4" /></td>
                                <td><label>Log Protocol</label><br /><select data-bind="options: $root.protocols, optionsText: function (item) { if (item == 0) return 'All'; else if (item == 1) return 'UDP'; else if (item == 2) return 'TCP'; else return 'TLS'; }, value: Protocol" ></select></td>
                                <td colspan="2"></td>
                            </tr>
                            <tr data-bind="visible: $root.advancedSearch">
                                <td><label>EventId Code</label><br /><input type="text" data-bind="value: EventId_Code" /></td>
                                <td><label>EventId CodeSystemName</label><br /><input type="text" data-bind="value: EventId_CodeSystemName" /></td>
                                <td><label>EventId DisplayName</label><br /><input type="text" data-bind="value: EventId_DisplayName" /></td>
                                <td><label>EventActionCode</label><br /><input type="text" data-bind="value: EventActionCode" /></td>
                                <td colspan="2"><label>EventDateTime like</label><br /><input type="text" data-bind="value: EventDateTime" /></td>
                            </tr>
                            <tr data-bind="visible: $root.advancedSearch">
                                <td>
                                    <label>EventTypeCode Code</label><br />                                    
                                    <%--<input type="text" data-bind="jqAuto: { autoFocus: true }, jqAutoSource: $root.undestroyedSupportedLogs, jqAutoValue: EventTypeCode_Code,
                                                                jqAutoSourceLabel: 'Code', jqAutoSourceInputValue: 'Code', jqAutoSourceValue: 'Code'" />--%>
                                    <input type="text" data-bind="value: EventTypeCode_Code" />
                                </td>
                                <td><label>EventTypeCode CodeSystemName</label><br /><input type="text" data-bind="value: EventTypeCode_CodeSystemName" /></td>
                                <td>
                                    <label>EventTypeCode DisplayName</label>
                                    <br />                                    
                                    <%--<input type="text" data-bind="jqAuto: { autoFocus: true }, jqAutoSource: $root.undestroyedSupportedLogs, jqAutoValue: EventTypeCode_DisplayName,
                                                                jqAutoSourceLabel: 'DisplayName', jqAutoSourceInputValue: 'DisplayName', jqAutoSourceValue: 'DisplayName'" />--%>
                                    <input type="text" data-bind="value: EventTypeCode_DisplayName" />                                    
                                </td>
                                <td colspan="3"><label>EventOutcomeIndicator</label><br /><input type="text" data-bind="value: EventOutcomeIndicator" /></td>
                            </tr>
                            <tr data-bind="visible: $root.advancedSearch">
                                <td><label>Audit Source Id</label><br /><input type="text" data-bind="value: AuditSourceId" /></td>
                                <td><label>Audit Enterprise Id</label><br /><input type="text" data-bind="value: AuditEnterpriseSiteId" /></td>
                                <td><label>Audit SourceType Code</label><br /><input type="text" data-bind="value: AuditSourceTypeCode" /></td>
                                <td><label>Audit SourceType CodeSystemName</label><br /><input type="text" data-bind="value: AuditSourceCodeSystemName" /></td>
                                <td colspan="2"><label>Audit Source Display Name</label><br /><input type="text" data-bind="value: AuditSourceOriginalText" /></td>
                            </tr>
                            <tr data-bind="visible: $root.advancedSearch">
                                <td><label>Source User Id</label><br /><input type="text" data-bind="value: SourceUserId" /></td>
                                <td><label>Source Network Access Point Id</label><br /><input type="text" data-bind="value: SourceNetworkAccessPointId" /></td>
                                <td><label>Human Requestor User Id</label><br /><input type="text" data-bind="value: HumanRequestorUserId" /></td>
                                <td><label>Destination User Id</label><br /><input type="text" data-bind="value: DestinationUserId" /></td>
                                <td colspan="2"><label>Destination Network Access Point Id</label><br /><input type="text" data-bind="value: DestinationNetworkAccessPointId" /></td>
                            </tr>                            
                        </table>                        
                    </div>       
                    <div data-bind="visible: $root.showResultPane" class="innerSubContent">
                        <p style="height:5%; width:1130px">
                            <label><input type="checkbox" data-bind="checked: $root.showSearchPane" />Show search filters</label>
                            <label style="float:right" data-bind="visible: $root.isResultTableVisible">
                                <img data-bind="visible: $root.showLoading" src="images/loading_small.gif" />
                                <span data-bind="visible: $root.hideLoading, text: 'Showing ' + $root.lastIndex() + ' record(s)'" ></span>
                            </label>
                        </p>
                        <div id="validGrid" class="gridStyle" data-bind="visible: $root.isValidResultVisible, koGrid: $root.gridOptions"></div>
                        <div id="invalidGrid" class="gridStyle" data-bind="visible: $root.isInValidResultVisible, koGrid: $root.gridOptionsInvalid"></div>
                        <h4 data-bind="visible: $root.displayNoDataText" >No log found... please refine your search criteria!</h4>  
                    </div>
                    <div data-bind="visible: $root.showSingleResultPane" class="innerSubContent">
                        <div class="tabs">
                            <p>
                                <button style="width:auto" data-bind="click: $root.backToResults">Back to record(s) view</button>
                            </p>
                            <label><input type="radio" value="Raw" data-bind="checked:$root.recordViewFormat" />Xml View</label>
                            <label data-bind="visible: $root.searchFilter().IsValid"><input type="radio" value="Designer" data-bind="checked: $root.recordViewFormat" />Formatted View</label>                            
                        </div>
                        <hr />
                        <div data-bind="visible: $root.recordViewFormat() == 'Raw', with: $root.messageContent, event: { mouseup: $root.showDecodedText }" id="tab1" class="tabPane">
                            <textarea disabled="disabled" wrap="hard" class="message" data-bind="value: RawMessage" ></textarea>
                        </div>
                        <div data-bind="visible: $root.recordViewFormat() != 'Raw', with: $root.messageContent, event: { mouseup: $root.showDecodedText }" id="tab2" class="tabPane">
                            <%--<textarea disabled="disabled" wrap="hard" class="message" data-bind="value: $root.messageContentText" ></textarea>--%>
                            <div class="tabs">
                                <h3>Event Identification</h3>
                                <hr />
                                <p>
                                    <label data-bind="if: $root.isVisible(Message.Event.EventId)">
                                        <b>Event Id</b><br />
                                        <b>Code: </b><span data-bind="text: Message.Event.EventId.Code"></span>
                                        <b> Code System Name: </b><span data-bind="text: Message.Event.EventId.CodeSystemName"></span>
                                        <b> Display Name: </b><span data-bind="text: Message.Event.EventId.DisplayName"></span><br /><br />
                                    </label>                                
                                    <label data-bind="if: $root.isVisible(Message.Event.EventActionCode)">
                                        <b>Event Action Code</b>
                                        <span data-bind="text: Message.Event.EventActionCode"></span><br /><br />
                                    </label>
                                    <label data-bind="if: $root.isVisible(Message.Event.EventDateTime)">
                                        <b>Event Date Time</b>
                                        <span data-bind="text: Message.Event.EventDateTime"></span><br /><br />
                                    </label>
                                    <label data-bind="if: $root.isVisible(Message.Event.EventOutcomeIndicator)">
                                        <b>Outcome Indicator</b>
                                        <span data-bind="text: Message.Event.EventOutcomeIndicator"></span><br /><br />
                                    </label>
                                    <label data-bind="if: $root.isVisible(Message.Event.EventTypeCode)">
                                        <b>Event Type Code</b><br />
                                        <b>Code: </b><span data-bind="text: Message.Event.EventTypeCode.Code"></span>
                                        <b> Code System Name: </b><span data-bind="text: Message.Event.EventTypeCode.CodeSystemName"></span>
                                        <b> Display Name: </b><span data-bind="text: Message.Event.EventTypeCode.DisplayName"></span>
                                    </label>
                                </p>
                                <hr />
                                <div data-bind="if: $root.isVisible(Message.ActiveParticipants)" class="tabs">
                                    <h3>Participants</h3>
                                    <hr />
                                    <!-- ko foreach: Message.ActiveParticipants -->
                                    <div class="tabs">
                                        <p>
                                            <label data-bind="if: $root.isVisible(UserId)"><b>User Id: </b><span data-bind="text: UserId"></span></label>
                                            <label data-bind="if: $root.isVisible(AlternativeUserId)"><b> Alternative User Id: </b><span data-bind="text: AlternativeUserId"></span></label>
                                            <label data-bind="if: $root.isVisible(UserName)"><b> User Name: </b><span data-bind="text: UserName"></span></label>
                                            <label data-bind="if: $root.isVisible(UserIsRequestor)"><b> Is Requestor: </b><span data-bind="text: UserIsRequestor"></span></label><br /><br />
                                            <label data-bind="if: $root.isVisible(RoleIdCode)">
                                                <b>Role Id Code</b><br />
                                                <b>Code: </b><span data-bind="text: RoleIdCode.Code"></span>
                                                <b> Code System Name: </b><span data-bind="text: RoleIdCode.CodeSystemName"></span>
                                                <b> Display Name: </b><span data-bind="text: RoleIdCode.DisplayName"></span><br /><br />
                                            </label>
                                            <label data-bind="if: $root.isVisible(NetworkAccessPointTypeCode)"><b>Network Access Point Type Code: </b><span data-bind="text: NetworkAccessPointTypeCode"></span></label>
                                            <label data-bind="if: $root.isVisible(NetworkAccessPointId)"><b> Network Access Point Id: </b><span data-bind="text: NetworkAccessPointId"></span></label>                                           
                                        </p>
                                        <hr />
                                    </div>
                                <!-- /ko -->
                                </div>
                                <div data-bind="if: $root.isVisible(Message.AuditSource)" class="tabs">
                                    <h3>Audit Source</h3>
                                    <hr />
                                    <p data-bind="with: Message.AuditSource">
                                        <label data-bind="if: $root.isVisible(AuditSourceId)"><b>User Id: </b><span data-bind="text: AuditSourceId"></span></label>
                                        <label data-bind="if: $root.isVisible(AuditEnterpriseSiteId)"><b> Alternative User Id: </b><span data-bind="text: AuditEnterpriseSiteId"></span></label><br />
                                        <label data-bind="if: $root.isVisible(Code)"><b>Code: </b><span data-bind="text: Code"></span></label>
                                        <label data-bind="if: $root.isVisible(CodeSystemName)"><b> Code System Name: </b><span data-bind="text: CodeSystemName"></span></label>
                                        <label data-bind="if: $root.isVisible(OriginalText)"><b> Original Text: </b><span data-bind="text: OriginalText"></span></label>
                                    </p>
                                    <hr />
                                </div>
                                <div data-bind="if: $root.isVisible(Message.ParticipantObjects)" class="tabs">
                                    <h3>Participant Objects</h3>
                                    <hr />
                                    <!-- ko foreach: Message.ParticipantObjects -->
                                    <div class="tabs">
                                        <p>
                                            <label data-bind="if: $root.isVisible(ParticipantObjectTypeCode)"><b>Type Code: </b><span data-bind="text: ParticipantObjectTypeCode"></span></label>
                                            <label data-bind="if: $root.isVisible(ParticipantObjectTypeCodeRole)"><b>Type Code Role: </b><span data-bind="text: ParticipantObjectTypeCodeRole"></span></label>
                                            <label data-bind="if: $root.isVisible(ParticipantObjectDataLifeCycle)"><b>Data Life Cycle: </b><span data-bind="text: ParticipantObjectDataLifeCycle"></span></label><br /><br />
                                            <label data-bind="if: $root.isVisible(ParticipantObjectIdTypeCode)">
                                                <b>ParticipantObjectIdTypeCode</b><br />
                                                <b>Code: </b><span data-bind="text: ParticipantObjectIdTypeCode.Code"></span>
                                                <b> Code System Name: </b><span data-bind="text: ParticipantObjectIdTypeCode.CodeSystemName"></span>
                                                <b> Display Name: </b><span data-bind="text: ParticipantObjectIdTypeCode.DisplayName"></span><br /><br />
                                            </label>
                                            <label data-bind="if: $root.isVisible(ParticipantObjectSensitivity)"><b>User Id: </b><span data-bind="    text: ParticipantObjectSensitivity"></span></label>
                                            <label data-bind="if: $root.isVisible(ParticipantObjectId)"><b> ObjectId: </b><span data-bind="    text: ParticipantObjectId"></span></label>
                                            <label data-bind="if: $root.isVisible(ParticipantObjectName)"><b> Name: </b><span data-bind="    text: ParticipantObjectName"></span></label><br />
                                            <label data-bind="if: $root.isVisible(ParticipantObjectDetail)">                                                
                                                <!-- ko foreach:ParticipantObjectDetail -->
                                                <label>
                                                    <b> Type: </b><span data-bind="text: Type"></span>
                                                    <b> Value: </b><span data-bind="text: Value"></span><br />
                                                </label>
                                                <!-- /ko -->                                                
                                            </label><br />
                                            <label data-bind="if: $root.isVisible(ParticipantObjectQuery)">
                                                <b> Query: </b><br />
                                                <textarea disabled="disabled" wrap="hard" class="querymessage" data-bind="value: window.atob(ParticipantObjectQuery)" ></textarea>
                                            </label>                                            
                                        </p>
                                        <hr />
                                    </div>
                                    <!-- /ko -->
                                </div>
                            </div>
                        </div>
                    </div>
				</div>
				<div class="innerContent" data-bind="with: schedulerProperty, visible: $root.showOperationData">
                    <h3>Maintain</h3>
					<table class="table">
                        <tr>
                            <td>
                                <label>Remove logs older than <input type="number" data-bind="value: $root.processRemoveDays" />days.</label>
						        <a href="#Action Center" data-bind="click: $root.RemoveLogs">Remove</a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <button data-bind="click: $root.ArchiveLogs">Archive</button>
                            </td>
                        </tr>					
					</table>
					<h3>Scheduler</h3>
                    <table class="table">
                        <thead>
                            <tr>
                                <th>
                                    <span style="float:left">Status</span>
                                    <a href="#Action Center" style="float:right;" data-bind="click: $root.refreshServerStatus"><img src="images/Refresh.png" alt="Refresh" /></a>
                                    <label style="float:right" class="checkBoxLabel"><input class="checkBox" type="checkbox" data-bind="checked: $root.autoRefreshChecked" />Auto refresh in 10 sec.</label>
                                    <br /><hr />
                                </th>
                            </tr>
                        </thead>
                        <tbody>                            
                            <tr>
                                <td>Last scheduler started at <span data-bind="text: new Date(parseInt(SchedulerStartDateTime.substr(6)))"></span></td>                                
                            </tr>
                            <tr>
                                <td><strong>Current Server Status: </strong><span data-bind="text: ActiveProcessStatus"></span></td>                                
                            </tr>
                        </tbody>
                    </table>
                    <p>
                        <input type="checkbox" data-bind="checked: IsRunning" /><label>Schedule archive of logs older than <input type="number" data-bind="    value: ArchiveDays" /> days.</label>
                        <button data-bind="click: $root.saveServerStatus">Save</button>
                    </p>
				</div>
				<div class="innerContent" data-bind="visible: showSupportedLogData">
					<h3>Supported log types</h3>
					<!--p>						
						<button id="btnRefreshLog" data-bind="click: refreshSupportedLogs" >Refresh</button>
					</p-->
                    <table class="tableWithHover">
                        <thead>
                            <tr>
                                <th style="width: 20%;">
                                    <span style="float:left">Event code</span><br />
                                    <input style="float:left" data-bind="value: newSupportedLogCode" placeholder="Code" />
                                </th>
                                <th style="width: 20%">
                                    <span style="float:left">Display name</span>
					                <input style="float:left" data-bind="value: newSupportedLogDisplayName" placeholder="Display Name" />
                                </th>
                                <th><br /><button style="float:left" data-bind="click: addLog">Add</button></th>
                            </tr>
                        </thead>
                        <tbody data-bind="foreach: supportedLogs">
                            <tr>
                                <td data-bind="text: Code" ></td>
                                <td data-bind="text: DisplayName" ></td>
                                <td><a href="#Supported Logs" data-bind="click: $root.removeSupportedLog">Remove</a></td>
                            </tr>
                        </tbody>                        
                    </table>
				</div>
				<div class="innerContent" data-bind="with: ports, visible: $root.showPortData">
					<h3>Manage ports</h3>
                    <table class="table">
                        <thead>
                            <tr>
                                <th style="width:15%"><span style="float:left">TCP Port</span></th>
                                <th style="width:15%"><span style="float:left">TLS Port</span></th>
                                <th><span style="float:left">UDP Port</span></th>
                            </tr>
                        </thead>
                        <tr>                            
                            <td>
                                <input type="number" data-bind="value: TCP" />
                            </td>
                            <td>
                                <input type="number" data-bind="value: TLS" />
                            </td>
                            <td>
                                <input type="number" data-bind="value: UDP" />
                            </td>
                        </tr>                        
                    </table>
					<p>
						<button data-bind="click: $root.savePorts">Save</button>
						<button data-bind="click: $root.refreshPorts">Refresh</button>
					</p>
				</div>
				<div class="innerContent" data-bind="visible: showPeriodData">
					<p></p>
                    <table class="table">
                        <thead>
                            <tr>
                                <th>Log Source</th>
                                <th>From</th>
                                <th>To</th>
                                <th>Type</th>
                            </tr>
                        </thead>
                        <tbody data-bind="foreach: periods">
                            <tr>
                                <td>
                                    <input type="radio" name="activeGroup" data-bind="value: name, checked: $root.selectedPeriod" /></td>
                                <td><span data-bind="text: startDateTime"></span></td>
                                <td><span data-bind="text: endDateTime"></span></td>
                                <td><span data-bind="text: dbType"></span></td>
                            </tr>
                        </tbody>
                    </table>	
					<p>
						<button data-bind="click: $root.setActiveDatabase">Save</button>
						<button data-bind="click: $root.refreshDatabaseList">Refresh</button>
					</p>
				</div>
			</div>
		</div>		
		<script>
		    var viewModel = new ARRViewModel();
		    ko.applyBindings(viewModel);
		</script>
	</body>
</html>
