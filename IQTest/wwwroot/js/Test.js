
function PreviousQuestion() {
    var previousQuestion = +$('#questionNumber').val() - 1;

    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Test/PreviousQuestion/?number=${previousQuestion}`,
        success: function (response) {
            $("#form-content").empty();
            $("#form-content").html(response);
            $('input:radio').on('click', function (e) {

                //e.currenTarget.value points to the property value of the 'clicked' target.

                var selectedChoice = e.currentTarget.value;

                var questionNumber = $('#questionNumber').val();


                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: `/Home/SaveAnswer/?number=${questionNumber}&selectedChoice=${selectedChoice}`,
                });


            });
        }
    });

}

function NextQuestion(qNumber) {

    var nextQuestion = +qNumber;

    if (qNumber == undefined) {
        nextQuestion = +$('#questionNumber').val() + 1;
    }

    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Test/GetQuestion/?number=${nextQuestion}`,
        success: function (response) {
            $("#form-content").empty();
            $("#form-content").html(response);
            $('input:radio').on('click', function (e) {

                //e.currenTarget.value points to the property value of the 'clicked' target.

                var selectedChoice = e.currentTarget.value;

                var questionNumber = $('#questionNumber').val();


                $.ajax({
                    type: 'POST',
                    cache: false,
                    url: `/Test/SaveAnswer/?number=${questionNumber}&selectedChoice=${selectedChoice}`,
                });


            });
        }
    });

}

function FinishTest() {

    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Test/FinishTest`,
        success: function (response) {
            $("#timer").empty();
            window.location.href = response.redirectToUrl;
        }
    });

}