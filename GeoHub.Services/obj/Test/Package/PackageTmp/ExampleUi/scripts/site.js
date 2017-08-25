'use strict';

var ExampleApp = (function () {
    var isInitialized = false;
    var coordinates;

    function init() {
        if (!isInitialized) {
            //wire up unhandled exception handler
            $(document).ajaxError(function (event, request, settings, thrownError) {
                var msg = thrownError ? thrownError : 'Error requesting page ' + settings.url;
                swal(
                   'ERROR!',
                   msg,
                   'error'
               );

            });
        }

        isInitialized = true;
    };

    function setUseLocation(val) {

        if (val && navigator.geolocation) {

            navigator.geolocation.getCurrentPosition(
                function (position) {
                    coordinates = position.coords;
                }
                );


        }
        else if (!navigator.geolocation) {
            throw {
                message: "Geolocation is not supported by this browser"
            }
        } else {
            coordinates = null;
        }
    };

    function wireUpAutoComplete(input) {
        var citiesUrl = 'http://localhost/GeoHub.Services/api/suggestions';

        var options = {
            url: function (phrase) {

                var url = citiesUrl + "?q=" + phrase;

                if (coordinates) {

                    url = url + "&latitude=" + coordinates.latitude + "&longitude=" + coordinates.longitude;
                }

                return url + "&type=json";

            },

            getValue: "Name",
            requestDelay: 500,
            ajaxSettings: {
                dataType: "json"
            },

            theme: "round",
            list: {
                onSelectItemEvent: function () {
                    var value = input.getSelectedItemData();

                    // alert(value);
                }
            }
        };

        input.easyAutocomplete(options);
    };
    return {
        Init: init,
        SetUseLocation: setUseLocation,
        WireUpAutoComplete: wireUpAutoComplete
    }
})();


