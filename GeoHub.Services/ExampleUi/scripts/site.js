'use strict';

var ExampleApp = (function() {
    var isInitialized = false;

    //holds results of "search near your location" checkbox
    var localCoordinates;
    var selectedSuggestion;


    function init() {
        if (!isInitialized) {
            //wire up unhandled exception handler
            $(document).ajaxError(function(event, request, settings, thrownError) {
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
                function(position) {
                    localCoordinates = position.coords;
                }
            );


        } else if (!navigator.geolocation) {
            throw {
                message: "Geolocation is not supported by this browser"
            }
        } else {
            localCoordinates = null;
        }
    };

    function getUrl(serviceMethod, q, latitude, longitude) {
        var apiBaseUrl = "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/";
        if (!serviceMethod) {
            throw {
                message: "serviceMethod is required"
            }
        }


        var url = apiBaseUrl + serviceMethod + "?type=json";

        if (q) {
            url = url + "&q=" + q;
        }

        if (latitude) {

            url = url + "&latitude = " + latitude + "&longitude = " + longitude;
        }

        return url;
    }

    function getLinkFromSelectedSuggestion(rel, name) {
        $.each(selectedSuggestion.Links, function(i, l) {
            if (l.Rel.toLowerCase() === rel.toLowerCase()) {
                return q ? l.Href + "&q=" + name : l.Href;
            }

        });
    }

    function wireUpAutoComplete(input) {

        //if input is cities and user has checked "use location"
        //return url including long and lat
        //else return url including only name search
        // 
        var options = {
            url: function(phrase) {

                if ($('#cities').is($(input))) {

                    if (!localCoordinates) {
                        return getUrl("suggestions", phrase);
                    } else {
                        return getUrl("suggestions", phrase, localCoordinates.latitude, localCoordinates.longitude);
                    }

                }
                if ($('#hospitals').is($(input))) {
                    return selectedSuggestion ? getLinkFromSelectedSuggestion("Hospitals", phrase) :
                        getUrl("Hospitals", phrase);


                } else {
                    return selectedSuggestion ? getLinkFromSelectedSuggestion("Airports", phrase) :
                        getUrl("Airports", phrase);
                }

            },

            getValue: function(element) {
                return element.Name;
            },
            requestDelay: 500,
            ajaxSettings: {
                dataType: "json"
            },

            theme: "round",
            list: {
                onSelectItemEvent: function() {
                    selectedSuggestion = input.getSelectedItemData();
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
