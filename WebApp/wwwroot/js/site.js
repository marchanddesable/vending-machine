
$('.machineBtn ul, .machineBtn li').on('click', (evt) => { evt.preventDefault(); });

$('.machineBtn').on('click', (evt) => {
    var prc = $(evt.currentTarget).find('.recipePrice');
    $('.scrn').html('<b>' + $(evt.currentTarget).data('rcpname') + '</b>: ' + $(evt.currentTarget).data('rcpprice'));
});

$('.machineBtn .buynow').on('click', (evt) => {

    var rcpid = $(evt.currentTarget).data('recipeid');
    var rcpname = $(evt.currentTarget).data('recipename');

    $('.msgArea').html('Voulez vous vraiment acheter cette boisson: ' + rcpname + ' ?');
    $('#btnyes').data('recipeid', rcpid);

    $('#btnyes').off('click').on('click', function () {

        var dts = { recipeid: rcpid };

        $.post("http://localhost:46356/api/buy", dts, function (data) {

            if (data.succeeded) {
                var tb = data.recipe.ingredients;

                for (i = 0; i < tb.length; i++) {
                    $('#stock-' + tb[i].id).html(tb[i].name + ' (' + tb[i].remainingUnits + ') - ' + tb[i].price);
                    $('#stock-' + tb[i].id).addClass('upd');
                }

                if (data.message) {
                    $('.infoArea').html(data.message);
                    $('#infoModal').modal('show');
                }
            }
            else if (data.message) {
                $('.infoArea').html(data.message);
                $('#infoModal').modal('show');
            }
        });

    });

    $('#buyModal').modal('show');

});