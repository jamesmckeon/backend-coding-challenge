'use strict';

var ExampleApp = (function () {
    var isInitialized = false;

    //holds results of "search near your location" checkbox
    var localCoordinates;

    //cache all returned suggestions for later retrieval of URLs
    var citySuggestions;
    var selectedSuggestion;

    //returns Base Url of current deployment
    function getApiBaseUrl() {

        return window.location.href.replace("ExampleUi/index.html", "api/");
    }


    function init() {
        if (!isInitialized) {


            //wire up unhandled exception handler
            $(document).ajaxError(function (event, xhr, settings, thrownError) {
                var msg = thrownError ? "Unexpected error: " + thrownError.message : 'Error requesting page ' + settings.url;
               
                console.warn("Response:",xhr.responseText);
                console.error(thrownError.toString());

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
                    localCoordinates = position.coords;
                }, function onError() { swal('ERROR', "Error getting user location", 'error'); }
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
        //var apiBaseUrl = "http://geo-complete-test.us-west-2.elasticbeanstalk.com/api/";
        var apiBaseUrl = getApiBaseUrl();
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

            url = url + "&latitude=" + latitude + "&longitude=" + longitude;
        }

        return url;
    }

    function getLinkFromSelectedSuggestion(rel, name) {

        var link;


        $.each(selectedSuggestion.Links, function (i, l) {
            if (l.Rel.toLowerCase() === rel.toLowerCase()) {
                //don't understand why json datatype in original select2 ajax wireup isn't 
                //getting sent to Api
               
                link =  (name ? l.Href + "&q=" + name : l.Href) +'&type=json';
            }

        });

        return link;
    }
    function setSelectedSuggestion(name) {

        var suggestion;
        $.each(citySuggestions, function (i, l) {
            if (l.Name.toLowerCase() === name.toLowerCase()) {
                suggestion = l;
                return;
            }

        });

        if (suggestion) {
            selectedSuggestion = suggestion;
        } else {
            throw 'Selected suggestion not found in suggestions cache';
        }
    }

    function getUrlForInput(input, searchTerm) {
        if ($('#cities').is($(input))) {

            if (!localCoordinates) {
                return getUrl("suggestions", searchTerm);
            } else {
                return getUrl("suggestions", searchTerm, localCoordinates.latitude, localCoordinates.longitude);
            }

        }
        if ($('#hospitals').is($(input))) {
            return selectedSuggestion ? getLinkFromSelectedSuggestion("Hospitals", searchTerm) :
                getUrl("Hospitals", searchTerm);


        } else {
            return selectedSuggestion ? getLinkFromSelectedSuggestion("Airports", searchTerm) :
                getUrl("Airports", searchTerm);
        }
    }

    
    function disable(input) {
        $(input).select2('enable', false);
    }

    function enable(input) {
        $(input).select2('enable', true);
    }

    function clear(input) {
        $(input).select2("val", "");
    }
    function wireUpAutoComplete(input) {

        //if input is cities and user has checked "use location"
        //return url including long and lat
        //else return url including only name search
        // 


        input.select2({
            width: '100%',
            allowClear: true,
            cache:false,
            multiple: false,
            maximumSelectionSize: 1,
            placeholder: "Start typing ....",
            minimumInputLength: 2,
       
            ajax: {
                url: function (params) {

                    var ret = getUrlForInput(input, params);
                    console.assert(ret, "url is null");
                    console.log(ret);
                    return ret;
                },
                quietMillis: 250,
             
                dataType: 'json',
                processResults: function (data) {
                    citySuggestions = data.Suggestions;
                    return {
                      
                        results: $.map(data.Suggestions, function (obj) {
                            return { id: obj.Name, text: obj.Name };
                        })
                    };
                }
            }
        });

        input.change(function(event) {

            
            if (!$(event.currentTarget).is($('#cities'))) {
                return;
            }

            var airports = $('#airports');
            var hospitals = $('#hospitals');

            if (event.val) {
                setSelectedSuggestion(event.val);
                enable(airports);
                enable(hospitals);
            } else {
                selectedSuggestion = null;
                clear(airports);
                clear(hospitals);
                disable(airports);
                disable(hospitals);
            }

        });

    };

    return {
        Init: init,
        SetUseLocation: setUseLocation,
        WireUpAutoComplete: wireUpAutoComplete,
        Enable: enable,
        Disable: disable
}
})();
