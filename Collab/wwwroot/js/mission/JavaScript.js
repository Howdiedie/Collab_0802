var $ = jQuery.noConflict();
$(function ($) {
    $("#sortable1, #sortable2,#sortable3").sortable({
        placeholder: "ui-state-highlight",
        connectWith: ".connectedSortable"
    }).disableSelection();
});
