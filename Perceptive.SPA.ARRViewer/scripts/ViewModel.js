function ActiveDatabase(data)
{
	this.isActive = ko.observable(data.IsActive);
	this.name = ko.observable(data.Name);
	this.startDateTime = ko.computed(function(){ if(isNaN(this.name().charAt(0))) return this.name(); else return new Date(ConvertToValidDateTime(this.name().split('_')[0])); }, this);
	this.endDateTime = ko.computed(function () { if (isNaN(this.name().charAt(0))) return ''; else return new Date(ConvertToValidDateTime(this.name().split('_')[1])); }, this);
	this.dbType = ko.computed(function () { if (this.name() == 'PerceptiveARR') return 'Active'; else return 'Archived'; }, this);
}

function ConvertToValidDateTime(data)
{
	return data.slice(0,13) + ":" + data.slice(13,15) + ":" + data.slice(15, 20) + ":" + data.slice(20, 22);
}

function SearchFilter(userName) {
    //Helper Data
    this.UserName = ko.observable(userName);
    this.LastStartingRowNumber = ko.observable();
    this.RetrieveNext = ko.observable();
    this.MaxLogsToRetrieve = ko.observable();

    //Log Recorder
    this.IPAddress = ko.observable();
    this.Protocol = ko.observable("All");
    this.LoggedFrom = ko.observable();
    this.LoggedTill = ko.observable();
    this.IsValid = ko.observable(true);

    //Global Search
    this.SearchText = ko.observable();

    //Valid Logs
    this.HostName = ko.observable();
    this.AppName = ko.observable();
    this.SentAfter = ko.observable();
    this.SentBefore = ko.observable();

    //Event Identification
    this.EventId_Code = ko.observable();
    this.EventId_CodeSystemName = ko.observable();
    this.EventId_DisplayName = ko.observable();
    this.EventActionCode = ko.observable();
    this.EventDateTime = ko.observable();
    this.EventOutcomeIndicator = ko.observable();
    this.EventTypeCode_Code = ko.observable();
    this.EventTypeCode_CodeSystemName = ko.observable();
    this.EventTypeCode_DisplayName = ko.observable();

    //Audit Source Identification
    this.AuditSourceId = ko.observable();
    this.AuditEnterpriseSiteId = ko.observable();
    this.AuditSourceTypeCode = ko.observable();
    this.AuditSourceCodeSystemName = ko.observable();
    this.AuditSourceOriginalText = ko.observable();

    //Active Participant
    this.SourceUserId = ko.observable();
    this.SourceNetworkAccessPointId = ko.observable();
    this.HumanRequestorUserId = ko.observable();
    this.DestinationUserId = ko.observable();
    this.DestinationNetworkAccessPointId = ko.observable();

    //Participant Object
    this.ParticipantObjectId = ko.observable();
}

function ARRViewModel() {
    // Data	
    var self = this;
    self.protocols = [0, 1, 2, 3];
    self.validity = [true, false];
    self.NoOfItems = [20, 50, 100];
    self.chosenViewId = ko.observable();
	self.serviceUri = "http://" + window.location.host + "/ARRService/";
	self.menu = ko.observableArray([]);
	$.getJSON(self.serviceUri + "GetMenu/" + document.getElementById("txtWelcome").innerText, function (result) { self.menu(result); });

    //
	self.logged1 = ko.observable();
	self.logged2 = ko.observable();
	self.sent1 = ko.observable();
	self.sent2 = ko.observable();

    //Record Viewer
	self.searchFilter = ko.observable(new SearchFilter(document.getElementById("txtWelcome").innerText));
	self.lastIndex = ko.observable();
	self.auditLogs = ko.observableArray([]);	
	self.logsToDisplay = ko.observable(20);
	self.showSearchPane = ko.observable(true);
	self.showResultPane = ko.observable(false);
	self.showSingleResultPane = ko.observable(false);
	self.isResultTableVisible = ko.computed(function () {
	    return self.auditLogs().length > 0;
	});	
	self.isValidResultVisible = ko.computed(function () {
	    return self.isResultTableVisible() && self.searchFilter().IsValid();
	});
	self.isValidResultVisible.subscribe(function () {
	    self.refreshGrid(true);
	});
	self.isInValidResultVisible = ko.computed(function () {
	    return self.isResultTableVisible() && !self.searchFilter().IsValid();
	});
	self.isInValidResultVisible.subscribe(function () {
	    self.refreshGrid(false);
	});

	self.refreshGrid = function (isValid) {
	    //$(window).trigger('resize');
	    var id = (isValid) ? "#validGrid" : "#invalidGrid";
	    //alert(id);
	    var grid = ko.dataFor($(id).children()[0]); // Get an instance of the grid
	    //var grid = ko.dataFor($(id)); // Get an instance of the grid
	    window.kg.domUtilityService.UpdateGridLayout(grid); // Force the grid to recalculate its layout	    
	};

	self.displayNoDataText = ko.computed(function () { return !(self.auditLogs().length > 0); });
	
	self.messageContent = ko.observable();
	self.messageContentText = ko.computed(function () { return ko.toJSON(self.messageContent()); });
	self.recordViewFormat = ko.observable("Raw");
	self.viewFormats = ["Raw", "Designer"];
	self.advancedSearch = ko.observable(false);

	
    //Supported Log    
	self.supportedLogs = ko.observableArray([]);
    self.newSupportedLogCode = ko.observable();
	self.newSupportedLogDisplayName = ko.observable();	
	
	//Action Center
	self.schedulerProperty = ko.observable();
	self.processRemoveDays = ko.observable(10);
	
	//View specific visibility
	self.showRecordData = ko.observable();
	self.showOperationData = ko.observable();	
	self.showSupportedLogData = ko.observable();
	self.showSchedulerData = ko.observable();
	self.showPortData = ko.observable();
	self.showPeriodData = ko.observable();
	
	//Ports
	self.ports = ko.observable();
	
	//Periods
    self.periods = ko.observableArray([]);
	self.selectedPeriod = ko.observable();	
	
    // Operations
	
	//Supported Logs
	self.goToView = function (menu) {
	    location.hash = menu.Name;
    };	
	
    self.addLog = function() {
		if(self.newSupportedLogCode() != undefined && self.newSupportedLogDisplayName() != undefined 
			&& self.newSupportedLogCode().trim().length > 0 && self.newSupportedLogDisplayName().trim().length > 0)
		{
		    $.getJSON(self.serviceUri + "AddSupportedLogType/Code/" + self.newSupportedLogCode().trim() + "/DisplayName/" + self.newSupportedLogDisplayName().trim(),
                function (result)
                {
                    if (result)
                    {                        
                        self.getSupportedLogs(true);
                        self.newSupportedLogCode("");
                        self.newSupportedLogDisplayName("");
                    }   
                });
		}
    };
	
    self.removeSupportedLog = function (log)
    {
        $.getJSON(self.serviceUri + "DeleteSupportedLogType/" + log.Id, function (result) { if (result) self.getSupportedLogs(true); });
    };
	
	self.getSupportedLogs = function(forced) {		
		
		if(forced || self.supportedLogs().length == 0)
		{
			$.getJSON(self.serviceUri + "GetSupportedLogTypes", function(allData) { if(allData) self.supportedLogs(allData); });
		}
	};

    //To be called for auto complete in record viewer.
	self.getSupportedLogs(true);
	
	self.refreshSupportedLogs = function() {
        self.getSupportedLogs(true);
    }; 
	
    //Action Center

	self.autoRefreshChecked = ko.observable(false);
	self.autoRefreshChecked.subscribe(function (newValue) { self.somefunction(newValue); }, self);
	var timerVariable;
	self.somefunction = function (value)
	{
	    if (value)
	        timerVariable = setInterval(function () { self.getServerStatus(true); }, 10000);
	    else
	        clearInterval(timerVariable);	    
	}
	
	self.getServerStatus = function(forced) {
		if(forced || self.schedulerProperty() == undefined)
		{
		    $.getJSON(self.serviceUri + "Scheduler", function (allData) { self.schedulerProperty(allData); });
		}
	};
	
	self.saveServerStatus = function() {
		if(self.schedulerProperty().ArchiveDays == '' || isNaN(self.schedulerProperty().ArchiveDays))
			alert('Please enter a proper number to proceed');
		else
		{
		    if (confirm('Press OK to confirm server changes.')) {
		        $.ajax(self.serviceUri + "Scheduler", {
		            data: ko.toJSON(self.schedulerProperty),
		            type: "post", contentType: "application/json",
		            success: function (result) { self.getServerStatus(true); }
		        });
		    }
		}
    };
	
	self.refreshServerStatus = function() {
        self.getServerStatus(true);
	};

	self.ArchiveLogs = function () {
	    if (confirm('Press OK to confirm Archive.'))
	        $.getJSON(self.serviceUri + "ArchiveLogs/1", function (allData) { self.getServerStatus(true); });
	};

	self.RemoveLogs = function () {
	    if (self.processRemoveDays() == '' || isNaN(self.processRemoveDays()))
	        alert('Please enter a proper number to proceed');
	    else if (confirm('Press OK to confirm deleting the log(s).'))
	        $.getJSON(self.serviceUri + "DeleteLogs/" + self.processRemoveDays(),
                function (allData) { self.getServerStatus(true); });
	};	
	
	//Ports
	
	self.getPorts = function(forced) {
		if(forced || self.ports() == undefined)
		{
			$.getJSON(self.serviceUri + "PortNumbers", function(allData) { self.ports(allData); });
		}
	};
	
	self.savePorts = function() {
	    if (self.ports().TCP == '' || isNaN(self.ports().TCP) || self.ports().TLS == '' || isNaN(self.ports().TLS) || self.ports().UDP == '' || isNaN(self.ports().UDP))
			alert('Please enter a proper number to proceed');
		else
		{
			$.ajax(self.serviceUri + "PortNumbers", {
				data: ko.toJSON(self.ports),
				type: "post", contentType: "application/json",
				success: function(result) { self.getPorts(true); }
			});
		}
    };
	
	self.refreshPorts = function() {
        self.getPorts(true);
    };	
	
	//Period Selection	
	self.getDatabaseList = function(forced) {
		if(forced || self.periods() == undefined || self.periods().length == 0)
		{
		    $.getJSON(self.serviceUri + "Databases/" + self.searchFilter().UserName(),
				function(allData) 
				{
					var mappedDBs = $.map(allData, function(item) { return new ActiveDatabase(item) });
					self.periods(mappedDBs);
					self.selectedPeriod(self.selectCurrentDatabase());
				}
			);
		}
	};
	
	self.selectCurrentDatabase = function()
	{
		var currentSelection;
		ko.utils.arrayForEach(self.periods(), function(item)
		{ 
			if(item.isActive())
				currentSelection = item.name();
		});
		
		return currentSelection;
	};
	
	self.setActiveDatabase = function() {
		$.getJSON(self.serviceUri + "Databases/" + self.searchFilter().UserName() + "/ActiveDatabase/" + self.selectedPeriod(), 
				function(result) { self.getDatabaseList(true); });		
    };
	
	self.refreshDatabaseList = function() {
        self.getDatabaseList(true);
	};
	
	// Client-side routes
    Sammy(function() {
        this.get('#:view', function () {
            self.chosenViewId(this.params.view);
			
			//Render all divs invisible;
			self.showRecordData(false);
			self.showOperationData(false);
			self.showSupportedLogData(false);
			self.showSchedulerData(false);
			self.showPortData(false);
			self.showPeriodData(false);
			
			switch(self.chosenViewId())
			{
				case 'Record Viewer':
					self.showRecordData(true);
					break;
				case 'Action Center':
					self.getServerStatus(false);
					self.showOperationData(true);					
					break;
				case 'Supported Logs':
					self.getSupportedLogs(false);
					self.showSupportedLogData(true);					
					break;				
				case 'Port Management':
					self.getPorts(false);
					self.showPortData(true);
					break;
				case 'Period Selection':
					self.getDatabaseList(false);
					self.showPeriodData(true);
					break;
				default:
					break;
			}            
        });

    }).run();

    self.search = function () {
        if (self.logsToDisplay() == '' || isNaN(self.logsToDisplay())) {
            alert('Please enter proper number of logs to proceed');
        }
        else {
            self.showSearchPane(false);
            self.showResultPane(true);
            self.searchFilter().LastStartingRowNumber(0);
            self.searchFilter().RetrieveNext(true);
            self.auditLogs.removeAll();
            self.populateData();
        }
    };    

    self.showLoading = ko.observable(false);
    self.hideLoading = ko.computed(function () { return !self.showLoading() });
    self.populateData = function () {
        self.searchFilter().MaxLogsToRetrieve(parseInt(self.logsToDisplay()));
        
        if (self.logged1() != undefined && self.logged1() != '') {
            self.searchFilter().LoggedFrom("/Date(" + new Date(self.logged1()).getTime() + ")/");
        }
        else
            self.searchFilter().LoggedFrom(undefined);

        if (self.logged2() != undefined && self.logged2() != '') {
            self.searchFilter().LoggedTill("/Date(" + new Date(self.logged2()).getTime() + ")/");
        }
        else
            self.searchFilter().LoggedTill(undefined);

        if (self.sent1() != undefined && self.sent1() != '') {
            self.searchFilter().SentAfter("/Date(" + new Date(self.sent1()).getTime() + ")/");
        }
        else
            self.searchFilter().SentAfter(undefined);

        if (self.sent2() != undefined && self.sent2() != '') {
            self.searchFilter().SentBefore("/Date(" + new Date(self.sent2()).getTime() + ")/");
        }
        else
            self.searchFilter().SentBefore(undefined);

        if (self.searchFilter().LastStartingRowNumber() == -1)
            return;
        
        self.showLoading(true);
        
        $.ajax(self.serviceUri + "GetLogs", {
            data: ko.toJSON(self.searchFilter),
            type: "post", contentType: "application/json",
            success: function (result) {
                if (self.auditLogs().length == 0)
                    self.auditLogs(result);
                else
                    self.auditLogs.pushAll(result);                

                if (self.auditLogs().length > 0) {
                    if (self.searchFilter().LastStartingRowNumber() == 0)
                        self.searchFilter().LastStartingRowNumber(1);
                    else
                        self.searchFilter().LastStartingRowNumber(self.searchFilter().LastStartingRowNumber() + self.searchFilter().MaxLogsToRetrieve());
                }

                self.lastIndex(self.auditLogs().length);

                if(self.searchFilter().MaxLogsToRetrieve() > result.length)
                    self.searchFilter().LastStartingRowNumber(-1);                

                self.showLoading(false);
            }
        });
    };

    self.reset = function () {
        self.searchFilter(new SearchFilter(document.getElementById("txtWelcome").innerText));
        self.logged1(undefined);
        self.logged2(undefined);
        self.sent1(undefined);
        self.sent2(undefined);
    };

    self.goToLog = function (log) {
        self.showLog(log.LogId);
    };
    
    self.showLog = function (logId) {
        $.getJSON(self.serviceUri + "GetLogs/" + logId + "/UserId/" + self.searchFilter().UserName(), function (allData) { self.messageContent(allData); });
        self.showResultPane(false);
        if (!self.searchFilter().IsValid())
            self.recordViewFormat("Raw");
        self.showSingleResultPane(true);
    };

    self.backToResults = function () {
        self.showResultPane(true);
        self.showSingleResultPane(false);
    };
    
    //koGrid
    self.mySelections = ko.observableArray([]);

    self.gridOptions = {
        data: self.auditLogs,
        displaySelectionCheckbox: false,
        enableRowReordering: true,
        footerVisible: false,
        selectedItems: self.mySelections,
        multiSelect: false,
        columnDefs: [
            { field: 'RemoteIP', displayName: 'Remote IP', width: '10%' },
            { field: 'IsValid', displayName: 'IsValid', width: '6%' },            
            { field: 'LogType', displayName: 'Log Type', width: '8%' },            
            { field: 'IDDescription', displayName: 'ID Description', width: '10%' },
            { field: 'EventType', displayName: 'Event Type', width: '14%' },
            { field: 'ActionCode', displayName: 'Action Code', width: '9%' },
            {
                field: 'DateTime', displayName: 'Logged at', width: '41%',
                cellTemplate: '<span data-bind="text: new Date(parseInt(($parent.entity[\'DateTime\']).substr(6)))"></span>'
            }]

    };

    self.gridOptionsInvalid = {
        data: self.auditLogs,
        displaySelectionCheckbox: false,
        enableRowReordering: true,
        footerVisible: false,
        selectedItems: self.mySelections,
        multiSelect: false,
        columnDefs: [
            { field: 'RemoteIP', displayName: 'Remote IP', width: '8%' },
            { field: 'IsValid', displayName: 'IsValid', width: '8%' },
            { field: 'LogType', displayName: 'Log Type', width: '8%' },
            {
                field: 'DateTime', width: '74%', displayName: 'Logged at',
                cellTemplate: '<span data-bind="text: new Date(parseInt(($parent.entity[\'DateTime\']).substr(6)))"></span>'
            }]
    };
    
    self.selectionChanged = ko.computed(function ()
    {
        if (self.mySelections().length > 0) {
            self.showLog(self.mySelections()[0].LogId);
            ko.utils.arrayForEach(self.mySelections(), function (player) {
                player.__kg_selected__ = false;
            });
        }
    });

    self.scrolled = function (data, event) {

        var elem = event.target;
        if (elem.scrollTop == (elem.scrollHeight - elem.offsetHeight - 0)) {
            self.populateData();
        }
    };

    self.isVisible = function isEmpty(str) {
        return !(!str || 0 === str.length);
    };

    self.showDecodedText = function (data, event) {
        if (event.shiftKey)
        {
            var text = getSelectionText();
            try
            {
                if (text.length > 0)
                    alert(window.atob(text));
            }
            catch(e)
            {
                alert('Selected text is not in proper base-64 format.')
            }
        }
    };
};

ko.bindingHandlers.datetimepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize datepicker with some optional options
        var options = allBindingsAccessor().datetimepickerOptions || {};
        $(element).datetimepicker(options);

        //handle the field changing
        ko.utils.registerEventHandler(element, "change", function () {
            var observable = valueAccessor();
            observable($(element).datetimepicker("getDate"));
        });

        //handle disposal (if KO removes by the template binding)
        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $(element).datetimepicker("destroy");
        });

    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            current = $(element).datetimepicker("getDate");

        if (value - current !== 0) {
            console.log("setting", value);
            $(element).datetimepicker("setDate", value);
            console.log("just set", $(element).datetimepicker("getDate"));
        }
    }
};

//jqAuto -- main binding (should contain additional options to pass to autocomplete)
//jqAutoSource -- the array of choices
//jqAutoValue -- where to write the selected value
//jqAutoSourceLabel -- the property that should be displayed in the possible choices
//jqAutoSourceInputValue -- the property that should be displayed in the input box
//jqAutoSourceValue -- the property to use for the value
ko.bindingHandlers.jqAuto = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var options = valueAccessor() || {},
            allBindings = allBindingsAccessor(),
            unwrap = ko.utils.unwrapObservable,
            modelValue = allBindings.jqAutoValue,
            source = allBindings.jqAutoSource,
            valueProp = allBindings.jqAutoSourceValue,
            inputValueProp = allBindings.jqAutoSourceInputValue || valueProp,
            labelProp = allBindings.jqAutoSourceLabel || valueProp;

        //function that is shared by both select and change event handlers
        function writeValueToModel(valueToWrite) {
            if (ko.isWriteableObservable(modelValue)) {
                modelValue(valueToWrite);
            } else {  //write to non-observable
                if (allBindings['_ko_property_writers'] && allBindings['_ko_property_writers']['jqAutoValue'])
                    allBindings['_ko_property_writers']['jqAutoValue'](valueToWrite);
            }
        }

        //on a selection write the proper value to the model
        options.select = function (event, ui) {
            writeValueToModel(ui.item ? ui.item.actualValue : null);
        };

        //on a change, make sure that it is a valid value or clear out the model value
        options.change = function (event, ui) {
            var currentValue = $(element).val();
            var matchingItem = ko.utils.arrayFirst(unwrap(source), function (item) {
                return unwrap(inputValueProp ? item[inputValueProp] : item) === currentValue;
            });

            if (!matchingItem) {
                writeValueToModel(null);
            }
        }


        //handle the choices being updated in a DO, to decouple value updates from source (options) updates
        var mappedSource = ko.computed(function () {
            mapped = ko.utils.arrayMap(unwrap(source), function (item) {
                var result = {};
                result.label = labelProp ? unwrap(item[labelProp]) : unwrap(item).toString();  //show in pop-up choices
                result.value = inputValueProp ? unwrap(item[inputValueProp]) : unwrap(item).toString();  //show in input box
                result.actualValue = valueProp ? unwrap(item[valueProp]) : item;  //store in model
                return result;
            });
            return mapped;
        }, null, { disposeWhenNodeIsRemoved: element });

        //whenever the items that make up the source are updated, make sure that autocomplete knows it
        mappedSource.subscribe(function (newValue) {
            $(element).autocomplete("option", "source", newValue);
        });

        options.source = mappedSource();

        //initialize autocomplete
        $(element).autocomplete(options);
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        //update value based on a model change
        var allBindings = allBindingsAccessor(),
            unwrap = ko.utils.unwrapObservable,
            modelValue = unwrap(allBindings.jqAutoValue) || '',
            valueProp = allBindings.jqAutoSourceValue,
            inputValueProp = allBindings.jqAutoSourceInputValue || valueProp;

        //if we are writing a different property to the input than we are writing to the model, then locate the object
        if (valueProp && inputValueProp !== valueProp) {
            var source = unwrap(allBindings.jqAutoSource) || [];
            var modelValue = ko.utils.arrayFirst(source, function (item) {
                return unwrap(item[valueProp]) === modelValue;
            }) || {};  //probably don't need the || {}, but just protect against a bad value          
        }

        //update the element with the value that should be shown in the input
        $(element).val(modelValue && inputValueProp !== valueProp ? unwrap(modelValue[inputValueProp]) : modelValue.toString());
    }
};

ko.observableArray.fn.pushAll = function (valuesToPush) {
    var underlyingArray = this();
    this.valueWillMutate();
    ko.utils.arrayPushAll(underlyingArray, valuesToPush);
    this.valueHasMutated();
    return this;
};

function LoadDefaultPage() {
    if (window.location.hash == '')
        window.location.hash = '#Record Viewer';
};

function getSelectionText() {
    var text = "";
    if (window.getSelection) {
        text = window.getSelection().toString();
    } else if (document.selection && document.selection.type != "Control") {
        text = document.selection.createRange().text;
    }
    return text;
};
