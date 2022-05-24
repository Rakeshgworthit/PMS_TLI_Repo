function openModelpop(pageName, fieldname, fieldval) {
    $.loader({ content: "<img src='/Content/img/Preloader_3.gif' />" });    
    $("#modalcontent").html("<img src='/Content/img/Preloader_3.gif' />");    
    $('#mainModel').modal({ backdrop: 'static', keyboard: false });
    pageName = pageName + "?" + fieldname + "=" + fieldval;
    $("#modalcontent").load(pageName);
    $.loader('close');
}

function notify(msg, type) {
    notif({
        msg: msg,
        type: type,
        position: "center",
        opacity: 0.9,
        timeout: 2000
    });
}

    function reloadGrid(gridname)
    {
        MVCGrid.reloadGrid(""+ gridname +"");
    }


    function DeleteConfirm(pageName, fieldname, fieldval) {
        debugger;
        $.loader({ content: "<img src='/Content/img/Preloader_3.gif' />" });
      
        $('#confirmModel').modal({ backdrop: 'static', keyboard: false });
        pageName = pageName + "?" + fieldname + "=" + fieldval;
        $(".Deletebtn").attr("onclick", "window.location.href='" + pageName + "'");
        $.loader('close');
    }

