var $ = jQuery.noConflict();

$(function ($) {
    //把資料丟給
    $(".show").click(function () {
        infoModal.showModal();
        var missionName = $(this).data("mission-name")
        console.log(missionName)
        
    });
    $("#sortable1, #sortable2,#sortable3").sortable({
        placeholder: "ui-state-highlight",
        connectWith: ".connectedSortable",
        update: function (event, ui) {
            // 获取所有sortable1的li元素，并发送Ajax请求
            $("#sortable1 li.draggable-mission").each(function () {
                var missionName = $(this).data("mission-name");
                $.ajax({
                    type: "POST",
                    url: "/Mission/ActionName",
                    data: { missionName: missionName, misState: "新任務" },
                    success: function (response) {
                        // 在响应成功后，进行相关的操作
                        console.log("Ajax request succeeded.");
                    },
                    error: function (xhr, status, error) {
                        // 在出错时进行错误处理
                        console.error(error);
                    }
                });
            });
            // 获取所有sortable2的li元素，并发送Ajax请求
            $("#sortable2 li.draggable-mission").each(function () {
                var missionName = $(this).data("mission-name");
                $.ajax({
                    type: "POST",
                    url: "/Mission/ActionName",
                    data: { missionName: missionName, misState: "進行中" },
                    success: function (response) {
                        // 在响应成功后，进行相关的操作
                        console.log("Ajax request succeeded.");
                    },
                    error: function (xhr, status, error) {
                        // 在出错时进行错误处理
                        console.error(error);
                    }
                });
            });
            // 获取所有sortable3的li元素，并发送Ajax请求
            $("#sortable3 li.draggable-mission").each(function () {
                var missionName = $(this).data("mission-name");
                $.ajax({
                    type: "POST",
                    url: "/Mission/ActionName",
                    data: { missionName: missionName, misState: "已完成" },
                    success: function (response) {
                        // 在响应成功后，进行相关的操作
                        console.log("Ajax request succeeded.");
                    },
                    error: function (xhr, status, error) {
                        // 在出错时进行错误处理
                        console.error(error);
                    }
                });
            });
        }
    }).disableSelection();

    $("#selector").on("change", function () {
        $("#selector option:selected").each(function () {
            console.log($(this).val())
            var target = $(this).val()
            //$(".ui-state").css("display", "none");
            $(".ui-state").each(function () {
                var intentName = $(this).data("intent-name");
                console.log(intentName)
                if (intentName == target || target == "all") { $(this).css("display", "block") }
                else { $(this).css("display", "none") }
            })
            
        });
    });
});
