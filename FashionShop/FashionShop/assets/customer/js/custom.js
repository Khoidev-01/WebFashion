/*Template Name: StylInto
File Name: custom.js
Author Name: ThemeVault
Author URI: http://www.themevault.net/
License URI: http://www.themevault.net/license*/

jQuery(document).ready(function () {
    jQuery('.box-category .menu').find('li>ul')
    jQuery('.box-category .menu li i').on("click", function () {
        if (jQuery(this).hasClass('fa-minus')) {
            jQuery(this).removeClass('fa-minus').parent('li').find('> ul').slideToggle();
        }
        else {
            jQuery(this).addClass('fa-minus').parent('li').find('> ul').slideToggle();
        }
    });
    
    jQuery('.category').find('h3')
    jQuery('.category h3 i').on("click", function () {
        if (jQuery(this).hasClass('fa-angle-down')) {
            jQuery(this).removeClass('fa-angle-down').parents('.category').find('.box-content').slideToggle();
        }
        else {
            jQuery(this).addClass('fa-angle-down').parents('.category').find('.box-content').slideToggle();

        }
    });
    jQuery('.footer-block').find('h4')
    jQuery('.footer-block h4 i').on("click", function () {
        if (jQuery(this).hasClass('fa-angle-down')) {
            jQuery(this).removeClass('fa-angle-down').parents('.footer-block').find('.list-unstyled').slideToggle();
        }
        else {
            jQuery(this).addClass('fa-angle-down').parents('.footer-block').find('.list-unstyled').slideToggle();

        }
    });
    
    $('.dropdown-menu').click(function(event){
     event.stopPropagation();
 })
});
$(document).ready(function() {
  
  $(window).scroll(function () {
      //if you hard code, then use console
      //.log to determine when you want the 
      //nav bar to stick.  
    if ($(window).scrollTop() > 150) {
      $('.main-nav-block').addClass('navbar-fixed');
    }
    if ($(window).scrollTop() < 151) {
      $('.main-nav-block').removeClass('navbar-fixed');
    }
  });
});

$(document).ready(function(){ 
    $(window).scroll(function(){ 
        if ($(this).scrollTop() > 100) { 
            $('#scroll').fadeIn(); 
        } else { 
            $('#scroll').fadeOut(); 
        } 
    }); 
    $('#scroll').click(function(){ 
        $("html, body").animate({ scrollTop: 0 }, 600); 
        return false; 
    }); 
});

//$(window).load(function() {
//    // start up after 2sec no matter what
//    window.setTimeout(function(){
//        $('body').removeClass("loading").addClass('loaded');
//    }, 1500);
//});




