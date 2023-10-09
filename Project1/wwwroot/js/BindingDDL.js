$(document).ready(function () {
    console.log("ready");
    //$('#State_Id').attr('disabled', true);
    //$('#City_Id').attr('disabled', true);


    $('#Country_Id').change(function () {
        //alert($('option:selected', this).text()); GetCities?stateId=4   GetStates?countryId=2
        var country_id = $('option:selected', this).val();

        $.ajax({
            url: '/Users/GetStates?countryId=' + country_id,
            success: function (response) {
                console.log(response);

                // Remove all old entries from State
                $('#State_Id').children().remove().end();
                $('<option>').val("").text("--Select State--").appendTo('#State_Id');
                $.each(response, function (key, value) {
                    $('<option>').val(value.id).text(value.stateName).appendTo('#State_Id');
                });

            }
        });

    });

    $('#State_Id').change(function () {
        var state_Id = $('option:selected', this).val();

        $.ajax({
            url: '/Users/GetCities?stateId=' + state_Id,
            success: function (response) {
                console.log(response);

                // Remove all old entries from State
                $('#City_Id').children().remove().end();
                $('<option>').val("").text("--Select City--").appendTo('#City_Id');
                $.each(response, function (key, value) {
                    $('<option>').val(value.id).text(value.cityName).appendTo('#City_Id');
                });

            }
        });
    });

    $('#City_Id').change(function () {
        //alert($('option:selected', this).val());
    });
});