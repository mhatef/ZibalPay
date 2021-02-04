
!function($) {
    "use strict";

    var SweetAlert = function() {};

    //examples 
    SweetAlert.prototype.init = function() {
        
    //Basic
    $('#sa-basic').click(function(){
        swal("پیام در اینجا قرار می گیرد");
    });

    //A title with a text under
    $('#sa-title').click(function(){
        swal("پیام در اینجا قرار می گیرد!", "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است.")
    });

    //Success Message
    $('#sa-success').click(function(){
        swal("سلام وقت بخیر", "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ و با استفاده از طراحان گرافیک است. چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است.", "success")
    });

    //Warning Message
    $('#sa-warning').click(function(){
        swal({   
            title: "آیا مطمئن هستید؟",   
            text: "شما پس از حذف فایل دیگر نمی توانید آن را بازیابی کنید؟",   
            type: "warning",   
            showCancelButton: true,   
            confirmButtonColor: "#DD6B55",   
            confirmButtonText: "بله میخواهم حذف شود",   
            closeOnConfirm: false 
        }, function(){   
            swal("حذف شد!", "فایل خیالی شما حذف شده است.", "success"); 
        });
    });

    //Parameter
    $('#sa-params').click(function(){
        swal({   
            title: "آیا مطمئن هستید؟",   
            text: "شما پس از حذف فایل دیگر نمی توانید آن را بازیابی کنید؟",   
            type: "warning",   
            showCancelButton: true,   
            confirmButtonColor: "#DD6B55",   
            confirmButtonText: "بله حذف شود!",   
            cancelButtonText: "نه انصراف میدهم",   
            closeOnConfirm: false,   
            closeOnCancel: false 
        }, function(isConfirm){   
            if (isConfirm) {     
                swal("حذف شد!", "فایل خیالی شما حذف شده است.", "success");   
            } else {     
                swal("متوقف شد", "فایل خیالی شما امن است :)", "error");   
            } 
        });
    });

    //Custom Image
    $('#sa-image').click(function(){
        swal({   
            title: "میلاد صفایی!",   
            text: "اخیرا به اینستاگرام پیوسته است",   
            imageUrl: "../assets/images/users/1.jpg" 
        });
    });

    //Auto Close Timer
    $('#sa-close').click(function(){
         swal({   
            title: "این هشدار به صورت خودکار بسته خواهد شد",   
            text: "بعد از 2 ثانیه بسته خواهد شد",   
            timer: 2000,   
            showConfirmButton: false 
        });
    });


    },
    //init
    $.SweetAlert = new SweetAlert, $.SweetAlert.Constructor = SweetAlert
}(window.jQuery),

//initializing 
function($) {
    "use strict";
    $.SweetAlert.init()
}(window.jQuery);