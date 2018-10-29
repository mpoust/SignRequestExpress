$('select[name="brandSelect"]').change(function () {
    if ($(this).val() != "Select Brand") {
        $('input[name="template"]').prop('disabled', false);
    }
    else {
        $('input[name="template"]').prop('disabled', true);
    }
});