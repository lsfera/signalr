//  signalr module



var signalrModule = (function (module, $, global) {
    $.connection.hub.url = "http://localhost:8181/signalr";
    var mediator = $(global.document),
        hub = $.connection.catalogHub;

    var drawAlarms = function () {
        var container = $("#chalkboard");

        var init = function () {
            container.empty();

            hub.client.onReceiveAlarms = function (data) {
                mediator.trigger("signalr/onReceiveAlarms", [data]);
                var toRemove = [], ids = $.map(data, function (key, value) {
                    return generationId(value);
                });

                $("#title").parent().fadeOut();

                container.find("li").each(function () {
                    var id = $(this).attr("id");
                    if ($.inArray(id, ids) !== -1) {
                        toRemove.push(id);
                    }
                });

                $.each(toRemove, function (key, value) {
                    $("#" + value, container).remove();
                });

                $.each(data, function (key, value) {
                    var el = $("#" + generationId(key), container);
                    if (el.length === 0) {
                        $(getTpl(key, value)).appendTo(container);
                    } else {
                        if (el.data().oldvalue != value) {
                            el.data("oldvalue", value);
                        }
                    }
                });
            };
        };

        function generationId(value) {
            return value.split('@')[0];
        }

        function getTpl(key, value) {
            var division = key.split('@')[1] || "",
             version= key.split('@')[2] || "",
             environment = key.split('@')[3] || "",
             title = division + ' | ' +environment,
             label = key.split('@')[4] || "",
                fullLabel = new String(label),
                maxLen = "DOLCEEGABBANA_500".length;

            if (label.length > maxLen) {
                label = label.substring(0, maxLen - 1) + "..";
            }

            
            return '<li title="' + fullLabel + '" style="display: inline-table" data-oldvalue="' + value + '" id="' + generationId(key) + '"><div style="width: 160px;" class="thumbnail"><p style="text-align:center;">' + title +'</p><center><a href="/alerts?Context=' + key.split('@')[0] + '" target="_blank"><img style="width: 66px;" src="../img/' + value + '.png"/></a></center><p style="text-align:center;">' + label + '</p></div></li>';
        }

        init();
    };

    //  public api
    return {
        start: function (defaults) {
            $.connection.hub.start().done(function () {
                mediator.trigger("signalr/done");
            }).fail(function () {
                mediator.trigger("signalr/fail");
            });
            return this;
        },
        drawAlarms: drawAlarms
    };

}(window.signalrModule || {}, jQuery, window));