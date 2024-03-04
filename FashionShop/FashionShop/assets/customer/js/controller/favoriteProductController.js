var favorite = {
    init: function () {
        favorite.registerEvents();
    },
    registerEvents: function () {
        $('.btnAddProductFavorite').off('click').on('click', function (e) {
            e.preventDefault();

            var productId = parseInt($(this).data('id'));
            favorite.addFavoriteProduct(productId);
        });
        $('.btnDeleteProductFavorite').off('click').on('click', function (e) {
            e.preventDefault();

            var productId = parseInt($(this).data('id'));
            favorite.deleteFavoriteProduct(productId);
        })
    },
    addFavoriteProduct: function (productID) {
        $.ajax({
            url: '/Account/AddFavoriteProduct',
            data: {
                ProductID: productID
            },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    //alert('Đã thêm sản phẩm vào danh sách yêu thích');
                }
                else {
                    //alert('Vui lòng đăng nhập');
                }
            }
        });
    },
    deleteFavoriteProduct: function (productID) {
        $.ajax({
            url: '/Account/DeleteFavoriteProduct',
            data: {
                ProductID: productID
            },
            type: 'POST',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    //alert('Đã xóa sản phẩm khỏi danh sách yêu thích');
                    location.reload(true);
                }
                else {
                    alert('Lỗi');
                }
            }
        });
    }
}

favorite.init();