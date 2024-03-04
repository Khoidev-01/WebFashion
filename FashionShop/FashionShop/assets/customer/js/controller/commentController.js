var comment = {
    init: function () {
        comment.registerEvents();
    },
    registerEvents: function () {
        $('#btnCommentNew').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var productID = btn.data('productid');
            var userID = btn.data('userid');            

            var commentmsg = $('#txtCommentNew').val();

            if (commentmsg == "") {
                alert("Bình luận không được để trống");
                return;
            }
            console.log(productID);
            console.log(userID);
            $.ajax({
                url: "/Product/AddNewComment",
                data: {
                    ProductID: productID,
                    UserID: userID,
                    ParentID: 0,
                    Content: commentmsg
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        //alert('Đã thêm bình luận');

                        location.reload(true);
                    }
                    else {
                        alert('Thêm bình luận lỗi');
                    }
                }
            });
        });

        $('.btnReply').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);

            var productID = btn.data('productid');
            var userID = btn.data('userid');
            var parentID = btn.data('parentid');

            var commentmsg = btn.data('content');
            var commentMsgValue = $('#' + commentmsg).val();

            if (commentMsgValue == "") {
                alert("Bình luận không được để trống");
                return;
            }
            $.ajax({
                url: "/Product/AddNewComment",
                data: {
                    ProductID: productID,
                    UserID: userID,
                    ParentID: parentID,
                    Content: commentMsgValue
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    if (response.status == true) {
                        //alert('Đã thêm bình luận');

                        location.reload(true);
                    }
                    else {
                        alert('Thêm bình luận lỗi');
                    }
                }
            });
        });

    }
};

comment.init();