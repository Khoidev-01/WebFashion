var product = {
    init: function () {
        product.registerEvents();
    },
    registerEvents: function () {
        $('#orderBy').off('change').on('change', function () {
            var idCategory = $(this).data('id');
            var idOrderProduct = $(this).val();

            var link = "/Product/Index/?idCategory=" + idCategory + "&idOrderProduct=" + idOrderProduct;
            window.location.href = link;
        });
    },
}

product.init();