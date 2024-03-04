var common = {
    init: function () {
        common.registerEvents();
    },
    registerEvents: function () {
        $("#txtKeyword").autocomplete({
            minLength: 1,
            source: function (request, response) {
                $.ajax({
                    url: "/Home/GetListProductByName",
                    dataType: "json",
                    data: {
                        keyword: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            //focus: function (event, ui) {
            //    $("#txtKeyword").val(ui.item.name);
            //    return false;
            //},
            //// Tự động gán giá trị khi hover title
            select: function (event, ui) {
                $("#txtKeyword").val(ui.item.name);
                //$("#project-id").val(ui.item.value);
                //$("#project-description").html(ui.item.desc);
                //$("#project-icon").attr("src", "images/" + ui.item.icon);

                return false;
            }
        }).autocomplete("instance")._renderItem = function (ul, item) {
            console.log(item);
            //if (item.length() == 0) {
            //    return $('<li style="font-size: 12px">')
            //        .append('<span>Không tìm thấy sản phẩm nào</span>')
            //        .appendTo(ul);
            //}

            var priceNew = 0;
            if (item.discount > 0) {
                priceNew = item.price - (item.price * item.discount / 100);

                return $('<li style="font-size: 12px">')
                    .append('<div class="result-product"> <img src="/' + item.image + '" style="height: 60px"/> <div class="list-result-product"> <p> ' + item.name + '</p> <span style="text-decoration: line-through; font-size: 80%"> ' + numeral(item.price).format('0,0') + '₫' + '</span> <span> ' + numeral(priceNew).format('0,0') + '₫' + '</span> </div>')
                    .appendTo(ul);

            }
            else {
                return $('<li style="font-size: 12px">')
                    .append('<div class="result-product"> <img src="/' + item.image + '" style="height: 60px"/> <div class="list-result-product"> <p> ' + item.name + '</p> <span> ' + numeral(item.price).format('0,0') + '₫' + '</span> </div>')
                    .appendTo(ul);
            }
        };
    }
}

common.init();