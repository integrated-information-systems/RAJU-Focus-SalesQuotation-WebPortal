
$(document).ready(function () {
    BindDocInternalId();
    BindSAPDocId();
    BindBPCode();
    BindBPName();
    BindIndenter();
    BindItemCode();
    BindItemName();
    $("#MainContent_txtPostingDate").datepicker({ dateFormat: "yy-mm-dd",
        onSelect: function (dateText) {
            __doPostBack('txtPostingDate', dateText);
        }
    });

    $("#MainContent_txtValidUntil").datepicker({ dateFormat: "yy-mm-dd",
        onSelect: function (dateText) {
            // __doPostBack('txtValidUntil', dateText);
        }
    });
    $("#MainContent_txtDocDate").datepicker({ dateFormat: "yy-mm-dd",
        onSelect: function (dateText) {
            //   __doPostBack('txtDocDate', dateText);
        }
    });

    $("#MainContent_FromDate").datepicker({ dateFormat: "yy-mm-dd",
        onSelect: function (dateText) {
            //   __doPostBack('txtDocDate', dateText);
        }
    });

    $("#MainContent_ToDate").datepicker({ dateFormat: "yy-mm-dd",
        onSelect: function (dateText) {
            //   __doPostBack('txtDocDate', dateText);
        }
    });


});


function BindBPCode() {
    $("#MainContent_txtBPCode").autocomplete({
        source: function (request, response) {
            // get BP Codes for auto complete
            var stri=request.term;
				stri=stri.replace("\"",'\\"'); 
            $.ajax({
                url: "../WebService/WebService.asmx/GetBPCodes",
                data: '{ "sLookUP": "' + stri + '"}',
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return { value: item }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        select: function (event, ui) {

				var str=ui.item.value;
				str=str.replace("\"",'\\"');
            // get corresponding BP Name after selecting item from auto complete
            $.ajax({
                url: "../WebService/WebService.asmx/GetBPName",
                type: "post",
                dataType: "json",
                contentType: "application/json; charset=utf-8",               
                data: '{ "sLookUP": "' + str + '"}',
                success: function (data) {
                    $("#MainContent_txtBPName").val(data.d);


                    //BindSlpNames(ui.item.value);
                    __doPostBack('txtBPCode', ui.item.value);
                },
                error: function () {
                }
            });

        },
        minLength: 1    // MINIMUM 1 CHARACTER TO START WITH.
    });
}



function BindBPName() {
    $("#MainContent_txtBPName").autocomplete({
        source: function (request, response) {
        
			var str=request.term;
			str=str.replace("\"",'\\"');
            $.ajax({
                url: "../WebService/WebService.asmx/GetBPNames",             
                data: '{ "sLookUP": "' + str + '"}',
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return { value: item }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        select: function (event, ui) {
            var paramenter = ui.item.value
			paramenter=paramenter.replace("\"",'\\"');
            $.ajax({
                url: "../WebService/WebService.asmx/GetBPCode",
                type: "post",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{ "sLookUP": "' + paramenter + '"}',
                success: function (data) {
                    $("#MainContent_txtBPCode").val(data.d);

                    //BindSlpNames(ui.item.value);

                    __doPostBack('txtBPName', ui.item.value);
                },
                error: function () {
                }
            });

        },
        minLength: 1    // MINIMUM 1 CHARACTER TO START WITH.
    });
}

function BindIndenter() {
    $("#MainContent_txtEndUser").autocomplete({
        source: function (request, response) {
        
			var str=request.term;
			str=str.replace("\"",'\\"');
            $.ajax({
                url: "../WebService/WebService.asmx/GetBPNames",             
                data: '{ "sLookUP": "' + str + '"}',
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return { value: item }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },        
        minLength: 1    // MINIMUM 1 CHARACTER TO START WITH.
    });
}
function BindSlpNames(bpcode) {

	var paramenter = bpcode;
			paramenter=paramenter.replace("\"",'\\"');
    // get salesperson Names
    $.ajax({
        url: "../WebService/WebService.asmx/GetSlpName",
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: '{ "sLookUP": "' + paramenter + '"}',
        success: function (data) {
            //$("#MainContent_txtBPName").val(data.d);
            var slps = data.d;
            $("#MainContent_LstCtPrsn").empty();
            $.each(slps, function (index, slp) {
                $("#MainContent_LstCtPrsn").append($("<option>").attr("value", slp.CntPrsnName).text(slp.CntPrsnName));
            });


        },
        error: function () {
        }
    });
}

function BindItemCode() {
    $("#MainContent_txtItemCode").autocomplete({
        source: function (request, response) {
            // get BP Codes for auto complete
            
            var paramenter = request.term;
			paramenter=paramenter.replace("\"",'\\"');
			
            $.ajax({
                url: "../WebService/WebService.asmx/GetItemCodes",             
                data: '{ "sLookUP": "' + paramenter + '"}',
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return { value: item }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        select: function (event, ui) {

			var str = ui.item.value;
			str = str.replace("\"",'\\"');
            // get corresponding BP Name after selecting item from auto complete
            $.ajax({
                url: "../WebService/WebService.asmx/GetItemName",
                type: "post",
                dataType: "json",
                contentType: "application/json; charset=utf-8",             
                data: '{ "sLookUP": "' + str + '"}',
                success: function (data) {
                    $("#MainContent_txtItemName").val(data.d);

                    //BindSlpNames(ui.item.value);
                           //             __doPostBack('txtItemName', ui.item.value);
                },
                error: function () {
                }
            });


            $.ajax({
                url: "../WebService/WebService.asmx/GetItemPrice",
                type: "post",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: "{ 'itemcode': '" + str + "','cardcode':'" + $("#MainContent_txtBPCode").val() + "'}",
                success: function (data) {
                  
				
                    $("#MainContent_txtItemPrice").val(data.d);
                    

                    //BindSlpNames(ui.item.value);
                    __doPostBack('txtItemCode', ui.item.value);
                },
                error: function () {
                }
            });



        },
        minLength: 1    // MINIMUM 1 CHARACTER TO START WITH.
    });
}

function BindItemName() {
    $("#MainContent_txtItemName").autocomplete({
        source: function (request, response) {
        
        var parameter = request.term ;
			parameter = parameter.replace("\"",'\\"');
            // get BP Codes for auto complete
            $.ajax({
                url: "../WebService/WebService.asmx/GetItemNames",            
                data: '{ "sLookUP": "' + parameter + '"}',
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return { value: item }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus);
                }
            });
        },
        select: function (event, ui) {
				var stri=ui.item.value;
				stri=stri.replace("\"",'\\"'); 
				
            // get corresponding Item Code after selecting item from auto complete
            $.ajax({
                url: "../WebService/WebService.asmx/GetItemCode",
                type: "post",
                dataType: "json",
                contentType: "application/json; charset=utf-8",              
                data: '{ "sLookUP": "' + stri + '"}',
                success: function (data) {
                    $("#MainContent_txtItemCode").val(data.d);

                    //BindSlpNames(ui.item.value);
                    //__doPostBack('txtItemName', ui.item.value);
                },
                error: function () {
                }
            });
			var str=ui.item.value;
			str = str.replace("\"",'\\"'); 
            $.ajax({
                url: "../WebService/WebService.asmx/GetItemPrice",
                type: "post",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: '{ "itemcode": "' + str + '","cardcode":"' + $("#MainContent_txtBPCode").val() + '"}',
                success: function (data) {
                    $("#MainContent_txtItemPrice").val(data.d);

                    //BindSlpNames(ui.item.value);
                    __doPostBack('txtItemCode', ui.item.value);
                },
                error: function () {
                }
            });


        },
        minLength: 1    // MINIMUM 1 CHARACTER TO START WITH.
    });
}

function BindDocInternalId() {

    $("#MainContent_txtDocNo").autocomplete({
        source: function (request, response) {
        
			var str=request.term;
			str = str.replace("\"",'\\"'); 
            $.ajax({
                url: "../WebService/WebService.asmx/GetInternalDocId",
                data: '{ "sLookUP": "' + str + '"}',
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return { value: item }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                  
                }
            });
        },
        select: function (event, ui) {
         //   __doPostBack('txtDocNo', ui.item.value);
        },
        minLength: 1    // MINIMUM 1 CHARACTER TO START WITH.
    });

}
function BindSAPDocId() {

    $("#MainContent_txtSAPDocNo").autocomplete({
        source: function (request, response) {
			var str=request.term;
			str = str.replace("\"",'\\"'); 
            $.ajax({
                url: "../WebService/WebService.asmx/GetSAPDocId",
                data: '{ "sLookUP": "' + str + '"}',
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return { value: item }
                    }))
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                  
                }
            });
        },
        select: function (event, ui) {
            //   __doPostBack('txtDocNo', ui.item.value);
        },
        minLength: 1    // MINIMUM 1 CHARACTER TO START WITH.
    });

}