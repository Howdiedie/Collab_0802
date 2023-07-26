var $ = jQuery.noConflict();
/*let infoModal = document.querySelector("#infoModal");*/
$(function ($) {
    //阻止mail的冒泡
    $(".mail").on("click", (event) => {
        event.stopPropagation(); // 阻止事件冒泡
    })
    //只把任務狀態丟給missionEdit
    $("#newTask").click(function () {
        infoModal.showModal();
        $("input[name='status'][value='新任務']").prop("checked", true);
    })
    $("#inProgress").click(function () {
        infoModal.showModal();
        $("input[name='status'][value='進行中']").prop("checked", true);
    })
    $("#completed").click(function () {
        infoModal.showModal();
        $("input[name='status'][value='已完成']").prop("checked", true);
    })
    //把全部資料丟給missionEdit
    $(".show").click(function () {
        infoModal.showModal();
        //取值
        var missionId = $(this).data("mission-id")
        var missionName = $(this).data("mission-name")
        var memberId = $(this).data("member-id")
        var startTime = $(this).data("start-time")
        var endTime = $(this).data("end-time")
        var misState = $(this).data("mis-state")
        var intentId = $(this).data("intent-id")
        var MisDescribe = $(this).data("mis-describe")
        //放值
        console.log(startTime, endTime)
        $("#missionId").val(missionId)
        $("#missionName").val(missionName)
        $("#memberStatus").val(memberId)
        $("#startDate").val(startTime)
        $("#endDate").val(endTime)
        $("input[name='status'][value='" + misState + "']").prop("checked", true);
        $("#statusSelect").val(intentId)
        $("#description").val(MisDescribe);
    });
    /*不會閃一下 但是有bug*/
    //$("#sortable1, #sortable2,#sortable3").sortable({
    //    placeholder: "ui-state-highlight",
    //    connectWith: ".connectedSortable",
    //    update: function (event, ui) {
    //        // 获取所有sortable1的li元素，并发送Ajax请求
    //        $("#sortable1 li.draggable-mission").each(function () {
    //            var missionName = $(this).data("mission-name");
    //            $.ajax({
    //                type: "POST",
    //                url: "/Mission/ActionName",
    //                data: { missionName: missionName, misState: "新任務" },
    //                success: function (response) {
    //                    // 在响应成功后，进行相关的操作
    //                    console.log("Ajax request succeeded.");
    //                },
    //                error: function (xhr, status, error) {
    //                    // 在出错时进行错误处理
    //                    console.error(error);
    //                }
    //            });
    //        });
    //        // 获取所有sortable2的li元素，并发送Ajax请求
    //        $("#sortable2 li.draggable-mission").each(function () {
    //            var missionName = $(this).data("mission-name");
    //            $.ajax({
    //                type: "POST",
    //                url: "/Mission/ActionName",
    //                data: { missionName: missionName, misState: "進行中" },
    //                success: function (response) {
    //                    // 在响应成功后，进行相关的操作
    //                    console.log("Ajax request succeeded.");
    //                },
    //                error: function (xhr, status, error) {
    //                    // 在出错时进行错误处理
    //                    console.error(error);
    //                }
    //            });
    //        });
    //        // 获取所有sortable3的li元素，并发送Ajax请求
    //        $("#sortable3 li.draggable-mission").each(function () {
    //            var missionName = $(this).data("mission-name");
    //            $.ajax({
    //                type: "POST",
    //                url: "/Mission/ActionName",
    //                data: { missionName: missionName, misState: "已完成" },
    //                success: function (response) {
    //                    // 在响应成功后，进行相关的操作
    //                    console.log("Ajax request succeeded.");
    //                },
    //                error: function (xhr, status, error) {
    //                    // 在出错时进行错误处理
    //                    console.error(error);
    //                }
    //            });
    //        });
    //    }
    //}).disableSelection();
    //}).disableSelection();

    /* 移動li 的post函式 */
    function sendAjaxRequest(missionName, misState) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                type: "POST",
                url: "/Mission/ActionName",
                data: { missionName: missionName, misState: misState },
                success: function (response) {
                    // 在响应成功后，进行相关的操作
                    console.log("Ajax request succeeded.");
                    resolve(); // 標記為成功
                },
                error: function (xhr, status, error) {
                    // 在出错时进行错误处理
                    console.error(error);
                    reject(error); // 標記為失敗
                }
            });
        });
    }
    //li可以移動的函式
    $("#sortable1, #sortable2, #sortable3").sortable({
        placeholder: "ui-state-highlight",
        connectWith: ".connectedSortable",
        update: function (event, ui) {
            var ajaxRequests = [];

            // 取得sortable1的li元素，並加入AJAX請求到陣列中
            $("#sortable1 li.draggable-mission").each(function () {
                var missionName = $(this).data("mission-name");
                ajaxRequests.push(sendAjaxRequest(missionName, "新任務"));
            });

            // 取得sortable2的li元素，並加入AJAX請求到陣列中
            $("#sortable2 li.draggable-mission").each(function () {
                var missionName = $(this).data("mission-name");
                ajaxRequests.push(sendAjaxRequest(missionName, "進行中"));
            });

            // 取得sortable3的li元素，並加入AJAX請求到陣列中
            $("#sortable3 li.draggable-mission").each(function () {
                var missionName = $(this).data("mission-name");
                ajaxRequests.push(sendAjaxRequest(missionName, "已完成"));
            });

            // 使用Promise.all等待所有的AJAX請求完成
            Promise.all(ajaxRequests)
                .then(function () {
                    // 所有的AJAX請求都成功完成後，執行這個程式區塊
                    console.log("All AJAX requests succeeded.");
                    // 重整頁面
                    window.location.href = window.location.href;
                })
                .catch(function (error) {
                    // 當有任何一個AJAX請求失敗時，執行這個程式區塊
                    console.error("At least one AJAX request failed:", error);
                    // 這裡您可以根據需要處理錯誤情況
                });
        }
    }).disableSelection();
    //select option 渲染頁面
    $("#selector").on("change", function () {
        $("#selector option:selected").each(function () {
            console.log($(this).val())
            var target = $(this).val()
            //$(".ui-state").css("display", "none");
            $(".ui-state").each(function () {
                var intentId = $(this).data("intent-id");
                console.log(intentId)
                if (intentId == target || target == "all") { $(this).css("display", "block") }
                else { $(this).css("display", "none") }
            })
            
        });
    });
});

/* 送出按鈕 將資料傳給controller */
$("#sendValue").click(function () {
    infoModal.close();
    var postMissionId = $("#missionId").val()
    var postMissionName = $("#missionName").val()
    var postMemberStatus = $("#memberStatus").val()
    var postStartDate = $("#startDate").val()
    var postEndDate = $("#endDate").val()
    var postSelectedValue = $("input[name='status']:checked").val();
    var postIntentSelect = $("#statusSelect").val()
    var postDescription = $("#description").val();
    console.log(postMissionId, postMissionName, postMemberStatus, postStartDate, postEndDate, postSelectedValue, postIntentSelect, postDescription)
    var data = {
        MissionId: parseInt(postMissionId),
        MissionName: postMissionName,
        MisStartTime:postStartDate,
        MisFinishTime:postEndDate,
        MisState: postSelectedValue,
        MisDescribe: postDescription,
        IntentId: parseInt(postIntentSelect),
        MemberId: parseInt(postMemberStatus)
    };
    console.log(JSON.stringify(data))
    $.ajax({
        url: "/MissionEdit/UpsertMission", // 請根據您的Controller路由進行調整
        type: "POST",
        data: data,
        success: function (result) {
            // 處理成功回傳的結果
            console.log(result);
            // alert視窗
            var message = '成功';
            if (message) {
                alert(message);
            }
            window.location.href = window.location.href;
        },
        error: function (xhr, status, error) {
            // 處理錯誤
            console.error("發生錯誤:", error);
        }
    });
    /* 清空 */
    $("#missionId").val("")
    $("#missionName").val("")
    $("#memberStatus").val("請選擇")
    $("#startDate").val("")
    $("#endDate").val("")
    $("#statusSelect").val("請選擇")
    $("#description").val("");
})
//控制 Modal開關
$(".close").on("click", () => {
    infoModal.close();  
    /* 清空 */
    $("#missionId").val("")
    $("#missionName").val("")
    $("#memberStatus").val("請選擇")
    $("#startDate").val("")
    $("#endDate").val("")
    $("#statusSelect").val("請選擇")
    $("#description").val("");
})
