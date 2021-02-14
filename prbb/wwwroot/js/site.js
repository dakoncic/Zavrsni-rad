
$(document).ready(function () {

    //FOLLOW PART
    ChangeTextOnElementWithClassName('moja', 'Following','isfollow');
    //FOLLOW2 PART
    ChangeTextOnElementWithClassName('clubme', 'Following','isfollow');
    //GOING1 PART
    ChangeTextOnElementWithClassName('goingmoja', 'Attending','isgoing');
    //GOING2 PART
    ChangeTextOnElementWithClassName('attendmoja', 'Attending','isgoing');
    //GOING3 PART
    ChangeTextOnElementWithClassName('goingEventInfo', 'Attending','isgoing');
});

//FOLLOW
jQuery('.moja').click(function () {
    changeTextForFollow('moja',this);

});
//FOLLOW2
jQuery('.clubme').click(function () {

    changeTextForFollow('clubme',this);
});
//GOING1
jQuery('.goingmoja').click(function () {
    changeTextForAttending('goingmoja',this);
});
//GOING2
jQuery('.attendmoja').click(function () {
    changeTextForAttending('attendmoja',this);
});
//GOING3
jQuery('.goingEventInfo').click(function () {
    changeTextForAttending('goingEventInfo',this);
});

function changeTextForAttending(className,ovaj) {
    var klass = document.getElementsByClassName(className);

    var eventid = $(ovaj).attr('data-eventid');
    var currentuserid = $(ovaj).attr('data-currentuserid');
    

    var isgoing = $(ovaj).attr('data-isgoing');
    if (isgoing == 'true') {
        $(ovaj).attr('data-isgoing', 'false');
        $(ovaj).css('backgroundColor', '#687fd9');
        $(ovaj).html('Attend');
    }
    else {
        $(ovaj).attr('data-isgoing', 'true');
        $(ovaj).css('backgroundColor', '#0e2996');
        $(ovaj).html('Attending');
    }

    $.ajax({
        url: 'https://' + location.host+'/Home/AddGoing',
        method: 'POST',
        data: {
            eventid: eventid,
            currentuserid: currentuserid
        }
,
        error: function () {
            alert('nije')
        }
    });
}

function changeTextForFollow(className,ovaj) {
    
    var clubid = $(ovaj).attr('data-clubid');
    var currentuserid = $("." + className).attr('data-currentuserid');
    var klass = document.getElementsByClassName(className);

    var isfollow = $(ovaj).attr('data-isfollow');

    if (isfollow == 'true') {
        $(ovaj).attr('data-isfollow', 'false');
        $(ovaj).css('backgroundColor', '#687fd9');
        $(ovaj).html('Follow');
    }
    else {
        $(ovaj).attr('data-isfollow', 'true');
        $(ovaj).css('backgroundColor', '#0e2996');
        $(ovaj).html('Following');
    }

    $.ajax({
        url: 'https://'+ location.host+'/Home/AddFollower',
        method: 'POST',
        data: {
            clubid: clubid,
            currentuserid: currentuserid
        },
        //success: function () {
        //    alert('uspjelo')
        //},

        error: function () {
            alert('nije')
        }
    });
}

function ChangeTextOnElementWithClassName(className, innerHtml, activity) {
    var array = [];
    $("." + className).each(function () {
        array.push($(this).attr("data-" + activity));
    });

    var klass = document.getElementsByClassName(className);
    for (var i = 0; i < klass.length; i++) {
        if (array[i] == 'true') {
            klass[i].style.backgroundColor = '#0e2996';
            klass[i].innerHTML = innerHtml;
        }
    }
}