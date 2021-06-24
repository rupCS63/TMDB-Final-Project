//// https://api.themoviedb.org/3/search/tv?api_key=1e5a5ee20af326aebb685a34a1868b76&language=en-US&page=1&include_adult=false&query=Grey%27s%20Anatomy

$(document).ready(function () {
    $("#getTV").click(getTV);
    $("#formUser").submit(postUser);
    $("#loginform").submit(loginUser);
    $("#disconnect-btn").hide();
    $("#disconnect-btn").click(disconnectUser);
    $("#admin-panel-btn1").hide();
    $('#quizz-btn1').hide();
    $('.chat').hide();

    //Randeer popular series by genre (35=comedy)
    //getFavSeries(35);
    $("#quizz-btn1").click(function () {
        sendQ();
    });

    $("#send-quiz").on("click", function () {
        checkAnswer($("input[name='choose']:checked").val())
    });

    user = localStorage.getItem("user-login");
    if (user != null) {
        enterUser(user);
    }

    episodnum = 0;

    // replace it with your own key -DONE
    key = "1e5a5ee20af326aebb685a34a1868b76";
    img_tmdb = "../Images/img_tmdb.png";
    url = "https://api.themoviedb.org/";
    imagePath = "https://image.tmdb.org/t/p/w500";
});

//Get the most popular series by genre
function getFavSeries(genre) {
    //int genre = genre code
    //https://api.themoviedb.org/3/discover/tv?api_key=1e5a5ee20af326aebb685a34a1868b76&sort_by=popularity.desc&with_genres=35

    let apiSearch = `https://api.themoviedb.org/3/discover/tv?api_key=1e5a5ee20af326aebb685a34a1868b76&sort_by=popularity.desc&with_genres= ${genre}`;

    ajaxCall("GET", apiSearch, "", getSeriesPopByGenreSuccessCB, getSeriesPopByGenreErrorCB)

}
function error() {

}

function getSeriesPopByGenreSuccessCB(seriesList) {
    console.log(seriesList);
}

function getSeriesPopByGenreErrorCB() {
    alert("Error");
}

function disconnectUser() {
    localStorage.clear();
    location.reload();

}

function postSeries(curr_tvshow) {
    let SeriesObj = {
        Id: curr_tvshow.id,
        Name: curr_tvshow.name,
        First_air_date: curr_tvshow.first_air_date,
        Origin_country: curr_tvshow.origin_country[0],
        Original_language: curr_tvshow.original_language,
        Overview: curr_tvshow.overview,
        Popularity: curr_tvshow.popularity,
        Poster_path: "https://image.tmdb.org/t/p/w500" + curr_tvshow.poster_path,
    };

    let api = "../api/Series";
    ajaxCall(
        "POST",
        api,
        JSON.stringify(SeriesObj),
        postSeriesSuccessCB,
        postSeriesErrorCB
    );
}

function postSeriesSuccessCB() {
    console.log("Series added")
    postEpisod(episodnum);
}

function postSeriesErrorCB() {
    console.log("Series error")

}

function userLoginToSystme(user) {
    localStorage.clear();
    delete user.Password;
    //console.log(JSON.stringify(user));
    localStorage.setItem("user-login", JSON.stringify(user));
    $("#register-btn").hide();
    $("#login-btn").hide();
    $("#disconnect-btn").show();

    $("#welcome-user").html(
        "<h6>Welcome " + user.Name + " " + user.LastName + " | Points: " + user.Points + "</h6>"
    );
    gUser = user
    gPoints = user.Points;
    if (user.IsAdmin == true) {
        $("#admin-panel-btn1").show();
    }
}

function enterUser(user) {
    user = JSON.parse(user);
    $("#register-btn").hide();
    $("#login-btn").hide();
    $("#disconnect-btn").show();
    gPoints = user.Points;
    gUser = user
    $("#welcome-user").html(
        "<h6>Welcome " + user.Name + " " + user.LastName + " | Points: "+user.Points+"</h6>"
    );
    if (user.IsAdmin == true) {
        $("#admin-panel-btn1").show();
    }
}


//../api/Users?mail=&password=..
function loginUser() {
    document.getElementById("id02").style.display = "none";
    let userlogin = $("#userlogin").val();
    console.log(userlogin);
    let passwordlogin = $("#passwordlogin").val();
    console.log(passwordlogin);
    api = "../api/Users?mail=" + userlogin + "&password=" + passwordlogin;
    console.log(api);
    ajaxCall("GET", api, "", loginUserSuccessCB, loginUserErrorCB);
    return false;
}

function loginUserErrorCB(e) {
    alert(e.responseJSON);
}

function loginUserSuccessCB(user) {
    console.log(user);
    if (user != null) {
        userLoginToSystme(user);
    } else if (user == null) {
        alert("Worng password or username");
    }
}

function postUser() {
    document.getElementById("id01").style.display = "none";

    let usrname = $("#usrname").val();
    let userlastname = $("#userlastname").val();
    let useremail = $("#useremail").val();
    let psw = $("#psw").val();
    let userphonenum = $("#userphonenum").val();
    let genderDDL = $("#genderDDL").val();
    let useryear = $("#useryear").val();
    let genreDDL = $("#genreDDL").val();
    let useraddress = $("#useraddress").val();
    if (genreDDL == "") {
        genreDDL == "Comedy";
    }
    let userObj = {
        Name: usrname,
        LastName: userlastname,
        Email: useremail,
        Password: psw,
        Cellphone: userphonenum,
        Gender: genderDDL,
        YearBirth: useryear,
        Genre: genreDDL,
        Address: useraddress,
        IsAdmin: 0,

    };

    //console.log(userObj)
    let api = "../api/Users";
    ajaxCall(
        "POST",
        api,
        JSON.stringify(userObj),
        postUserSuccessCB,
        postUserErrorCB
    );
    return false;
}

function postUserErrorCB() {
    alert("fail to add user");
}

function postUserSuccessCB() {
    alert("user added");
    //add to  localStorage
}

function getTV() {
    $(".container-tvshow").html("");
    let name = $("#tvShowName").val();
    let method = "3/search/tv?";
    api_key = "api_key=" + key;
    let moreParams = "&language=en-US&page=1&include_adult=false&";
    let query = "query=" + encodeURIComponent(name);
    let apiCall = url + method + api_key + moreParams + query;
    ajaxCall("GET", apiCall, "", getTVSuccessCB, getTVErrorCB);
}

function getTVSuccessCB(tv) {
    curr_tvshow = tv.results[0]
    tvId = tv.results[0].id;
    let poster = imagePath + tv.results[0].poster_path;
    str = "<img class='tv-show-img' src='" + poster + "'/>";
    $(".tv-show-image").html(str);//image of TV SHOW after search
    let method = "3/tv/";
    let api_key = "api_key=" + key;
    let apiCall = url + method + tvId + "?" + api_key; //^ change seasson 1 to multi

    ajaxCall("GET", apiCall, "", getSeasonSuccessCB, getSeasonErrorCB);
}

function renderSeason(season) {
    let year = JSON.parse(localStorage.getItem('user-login')).YearBirth
    let genre = JSON.parse(localStorage.getItem('user-login')).Genre
    $(".season-render").html(
        `<select onchange="getEpisode(this.value);getRecommendations(${year},'${genre}');" name="Seasons" id="seasonselect"></select>`
    );
    //getRecommendations(${year},${genre})
    //$("#seasonselect").append(`<option>Select</option>`)
    $(".tv-show-name").html(gSeason.name);
    $(".tv-show-overview").html(gSeason.overview);


    for (var i = 0; i < season.seasons.length; i++) {

        $("#seasonselect").append(
            "<option value=" +
            i +
            "|" +
            tvId +
            ">" +
            season.seasons[i].name +
            "</option>"
        );
    }
}

function getEpisode(value) {
    let temp = value.split("|");
    seasonNumber = temp[0];
    let tvID = temp[1];
    let method = "3/tv/";
    let method2 = "/season/";

    let apiCall =
        url +
        method +
        tvID +
        method2 +
        seasonNumber +
        "?" +
        api_key +
        "&language=en-US";

    ajaxCall("GET", apiCall, "", getEpisodeSuccessCB, getEpisodeErrorCB);
}

function getEpisodeSuccessCB(episod) {
    epi = episod;
    $(".render-episodes").html(`<div class="scrollbar" id="style-15">
            <div class="force-overflow">
            </div>
            </div>`);
    for (var i = 0; i < episod.episodes.length; i++) {
        x = JSON.stringify(curr_tvshow).split("'").join('')


        poster =
            "https://image.tmdb.org/t/p/w500" + episod.episodes[i].still_path;
        imgURL = "<img id='poster' src='" + poster + "'/>";
        $(".force-overflow").append(
            `<div class="episodecard"> ${imgURL} 
                        <h4 id="episod-name">${episod.episodes[i].name}</h4>
                        <h6 id="episod-date">${episod.episodes[i].air_date}</h6>
                        <button onclick='savenumber(${i});postSeries(${x})'; type='button' id="addtofav-btn" class='myButton'>Add to favorite</button>
                   
                    </div>`
        );
    }
}
function getRecommendations(yearOfBirth, favGenre) {
    apiCall = `../api/series?yearOfBirth=${yearOfBirth}&favGenre=${favGenre}`
    ajaxCall("GET", apiCall, "", getRecommendationsSuccessCB, error);
}

function getRecommendationsSuccessCB(recommendations) {
    //epi = episod;
    console.log("recommendations")
    let recommendations_to_render 
    console.log(recommendations[0].Id)
    //https://api.themoviedb.org/3/tv/1668/recommendations?api_key=1e5a5ee20af326aebb685a34a1868b76&language=en-US&page=1
    for (var i = 0; i < recommendations.length; i++) {
            recommend = recommendations[i].Id
            fetch("https://api.themoviedb.org/3/tv/"+recommend+"/recommendations?api_key=1e5a5ee20af326aebb685a34a1868b76&language=en-US&page=1")
                .then(response => response.json())
                .then(data => console.log(data))
                .catch(err => console.error(err));
            console.log(i)
            console.log(recommend)
    }

    //$(".render-recommendations").html(`<div class="scrollbar" id="style-15">
    //        <div class="force-overflow">
    //        </div>
    //        </div>`);
    //for (var i = 0; i < episod.episodes.length; i++) {
    //    x = JSON.stringify(curr_tvshow).split("'").join('')


    //    poster =
    //        "https://image.tmdb.org/t/p/w500" + episod.episodes[i].still_path;
    //    imgURL = "<img id='poster' src='" + poster + "'/>";
    //    $(".force-overflow").append(
    //        `<div class="episodecard"> ${imgURL} 
    //                    <h4 id="episod-name">${episod.episodes[i].name}</h4>
    //                    <h6 id="episod-date">${episod.episodes[i].air_date}</h6>
    //                    <button onclick='savenumber(${i});postSeries(${x})'; type='button' id="addtofav-btn" class='myButton'>Add to favorite</button>
                   
    //                </div>`
    //    );
    //}
}

function savenumber(i) {
    episodnum = i;
}

function postEpisod(i) {

    let episodeObj = {
        Id: epi.episodes[i].id,
        SeriesId: curr_tvshow.id,
        Name: gSeason.name,
        SeasonNumber: seasonNumber,
        EpisodeName: epi.episodes[i].name,
        Img: "https://image.tmdb.org/t/p/w500" + epi.episodes[i].still_path,
        Description: epi.episodes[i].overview,
        BroadcastDate: epi.episodes[i].air_date,
    };
    var user_id = JSON.parse(localStorage.getItem('user-login')).Id;
    let api = "../api/Episodes?id=" + user_id;
    ajaxCall("POST", api, JSON.stringify(episodeObj), postEpisodSuccessCB, postEpisodErrorCB);

}

function postEpisodSuccessCB(i) {
    if (i == 1) {
        alert("episod added")
    }
    else {
        alert("something worng / you already added this episod")
    }
}

function postEpisodErrorCB() {
    console.log(err);
}

function getEpisodeErrorCB() {
    console.log(err);
}

function getSeasonSuccessCB(season) {
    gSeason = season;
    renderChat(gSeason);
    renderSeason(season);
    initQuestionbtn();

}
function initQuestionbtn() {
    $('#quizz-btn1').show();
    $('#quizz-btn1').html(`Take Question about ${gSeason.name}`)
}

function getSeasonErrorCB(err) {
    console.log(err);
}

function getTVErrorCB(err) {
    console.log(err);
}

// CHAT START

function renderChat(gSeason) {
    initChat(gSeason)
    // const database = firebase.database()
    // database.ref('/series/' + gSeason.name).set({
    //     name: JSON.parse(localStorage.getItem('user-login')).Name,
    //     message: "Hello"
    // });

}


function initChat() {
    $('.chat').show();
    $("#chat-name").html(`${gSeason.name} Chat`)
    active = false;
    msgArr = [];
    chat = firebase.database().ref(gSeason.name);
    reder_messages = document.getElementById("chat-messages");
    $('#chat-name').val(gSeason.name + ' Chat')
    // msg = 'hey1'
    // chat.push().set({"msg":msg,"name":JSON.parse(localStorage.getItem('user-login')).Name});
    getMSGfromDB()
    // listen to incoming messages
    initSentBTN()
    listenToNewMessages()



}
function initSentBTN() {
    $("#chat-input").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#chat-send-btn").click();
        }
    });
}

function listenToNewMessages() {
    // child_added will be evoked for every child that was added
    // on the first entry, it will bring all the childs
    chat.on("child_added", snapshot => {
        msg = {
            name: snapshot.val().name,
            msg: snapshot.val().msg,
        }
        msgArr.push(msg)
        printMessage(msg);
    })
}
function printMessage(msg) {
    let str = `<div class="message">${msg.name}: ${msg.msg}</div>`;
    reder_messages.innerHTML += str;
}

function AddMSG() {
    let msg = $('#chat-input').val()
    if (msg === "") {
        return
    }
    let name = JSON.parse(localStorage.getItem('user-login')).Name
    chat.push().set({ "msg": msg, "name": name });
    $('#chat-input').val('')
    return
}

function getMSGfromDB() {
    msgArr = [];
    // once listens to an event and then deletes the listner
    // it is usually used to initially bring data
    chat.once("value", snapshot => {
        snapshot.forEach(element => {
            msg = {
                msg: element.val().msg,
                name: element.val().name,
            }
            msgArr.push(msg)
        });
        printMessages(msgArr);
    })

}

function printMessages(msgArr) {
    var str = "";
    for (let index = 0; index < msgArr.length; index++) {
        const msg = msgArr[index];
        str += `<div class="message">${msg.name}: ${msg.msg}</div>`
    }
    reder_messages.innerHTML = str;
}
// CHAT END



//Quiz:

var modal = document.getElementById("myModal");

// Get the button that opens the modal
var btn = document.getElementById("quizz-btn1");

// Get the <span> element that closes the modals
var span = document.getElementById("send-quiz");

// When the user clicks on the button, open the modal
btn.onclick = function () {
    modal.style.display = "block";
}

// When the user clicks on <span> (x), close the modal
span.onclick = function () {
    modal.style.display = "none";
}

// When the user clicks anywhere outside of the modal, close it
window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}






//return: obj: q,4 answers, answer.
function sendQ() {
    //type of qs:
    //1. number of seasons
    //2. first air date - year
    //3. one from the cast
    //4. what is the name of the charactor

    //generate type of q
    qType = Math.floor(Math.random() * 3) + 1;

    switch (qType) {
        case 1: //1. number of seasons
            objQ1 = {
                question: `How much season there are in ${gSeason.name} ?`,
                answer: gSeason.number_of_seasons.toString(),
                answers:
                    [(Math.floor(Math.random() * 10) + 1).toString(),
                    (Math.floor(Math.random() * 10) + 1).toString(),
                    (Math.floor(Math.random() * 10) + 1).toString(),
                    gSeason.number_of_seasons.toString()
                    ]
            };
            showQuestion(objQ1);
            console.log(objQ1);
            break;

        case 2: //2. first air date - year
            objQ2 = {
                question: `When the series was first lanuch?`,
                answer: gSeason.first_air_date.toString().substring(0, 4),
                answers: [(Math.floor(Math.random() * 61) + 1960).toString(),
                (Math.floor(Math.random() * 61) + 1960).toString(),
                (Math.floor(Math.random() * 61) + 1960).toString(),
                gSeason.first_air_date.toString().substring(0, 4)
                ]
            };
            showQuestion(objQ2);
            console.log(objQ2);
            break;

        case 3: //3. one from the cast
            objQ3 = {
                question: `How much episodes there are in ${gSeason.name}`,
                answer: gSeason.number_of_episodes.toString(),
                answers: [(Math.floor(Math.random() * 250) + 1).toString(),
                (Math.floor(Math.random() * 250) + 1).toString(),
                (Math.floor(Math.random() * 250) + 1).toString(),
                gSeason.number_of_episodes.toString()
                ]
            };
            showQuestion(objQ3);
            console.log(objQ3);
            break;

        default:
            alert("Error");
            break;
    }


}

function showQuestion(the_question) {
    question = the_question
    $(".question").html('');
    $(".answer-1").html('');
    $(".answer-2").html('');
    $(".answer-3").html('');
    $(".answer-4").html('');

    numbers = []

    while (numbers.length != 4) {

        let num = Math.floor(Math.random() * 4)

        if (!numbers.includes(num)) {
            numbers.push(num)
        }
    }

    $(".question").html(question.question);
    $(".answer-1").html(question.answers[numbers[0]]);
    $(".answer-2").html(question.answers[numbers[1]]);
    $(".answer-3").html(question.answers[numbers[2]]);
    $(".answer-4").html(question.answers[numbers[3]]);


    $(".choose-r-1").val(question.answers[numbers[0]]);
    $(".choose-r-2").val(question.answers[numbers[1]]);
    $(".choose-r-3").val(question.answers[numbers[2]]);
    $(".choose-r-4").val(question.answers[numbers[3]]);



}

function checkAnswer(user_answer) {
    console.log("user_answer:" + user_answer)
    console.log("the answer:" + question.answer)
    $(".choose").prop("checked", false);

    if (user_answer == question.answer) {
        swal("CORRECT !", "You gain 5 points !!", "success");
        api_add_points = "../api/Users?id=" + JSON.parse(localStorage.getItem('user-login')).Id
        ajaxCall("PUT", api_add_points, "", AddPointsSuccessCB, error)
        return false
    }
    else {
        swal("WORNG", "maybe try again :)", "error");
        return
    }

}
function AddPointsSuccessCB() {
    var getuser = localStorage.getItem('user-login')
    getuser = JSON.parse(getuser)
    getuser.Points += 5
    localStorage.setItem('user-login', JSON.stringify(getuser))
    $("#welcome-user").html(
        "<h6>Welcome " + gUser.Name + " " + gUser.LastName + " | Points: " + JSON.parse(localStorage.getItem('user-login')).Points + "</h6>"
    );
}

