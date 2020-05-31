var selected = [];
var categories;
var projects;
var currentCats = [];
var descending = true;

$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: 'GetAllProjectsAndCategories',
        success: function (data) {
            if (data != null) {
                projects = data.projects;
                categories = JSON.parse(data.categories);
                console.log(projects);
                if (projects.length > 0) {
                    renderCategories();
                    renderProjects();
                }
                else {
                    $(".project-row").empty();
                    var alertMsg = document.createElement("h2");
                    alertMsg.setAttribute("style", "color:black; margin-left:20px;");
                    alertMsg.innerText = "No projects yet! Come back soon!";
                    $(".project-row").append(alertMsg);

                }
               

            } else {
                console.log(data.msg);
            }
        }
    });
});

$('.sort-dropdown').click(function () {
    $('.sort-menu').toggle();

});

function reformatDate(date) {
    dateString = new Date(date);
    var newdate = dateString.getFullYear() + '-' + (dateString.getMonth() + 1) + '-' + dateString.getDate();
    var formated = new Date(newdate);
    return formated;

}

function renderProjects(sort) {
    $(".project-row").empty();
    if (sort != null) {
        descending = !descending;
        if (descending) {
            projects.sort((a, b) => reformatDate(b.date) - reformatDate(a.date));
            $('.sort-icon').removeClass('flip');

        } else {
            projects.sort((a, b) => reformatDate(a.date) - reformatDate(b.date));
            $('.sort-icon').addClass('flip');


        }
        
    }
    for (var key in projects) {

        if (projects.hasOwnProperty(key)) {
            var val = projects[key];
            var card = document.createElement("DIV");
            card.setAttribute("class", "project-card-wrapper");

            var content = document.createElement("DIV");
            content.setAttribute("class", "card-content");

            var title = document.createElement("h3");
            title.setAttribute("class", "project-card-title");
            title.innerText = val.name

            var cats = document.createElement("UL");
            cats.setAttribute("class", "project-ul");

            var link = document.createElement("a");
            link.setAttribute("class", "project-link");
            link.innerText = "View Entry";
            link.href = "https://localhost:44386/Home/Entry/" + val.id;


            var i = 0;
            for (var key in categories) {
                if (categories.hasOwnProperty(key)) {
                    if (categories[key].project == val.id) {
                        var cat = document.createElement("LI");
                        cat.setAttribute("class", "project-li");
                        cat.innerText = categories[key].name;
                        cats.appendChild(cat);
                    }
                }
            }

            dateString = new Date(val.date);
            var newdate = dateString.getFullYear() + '/' + (dateString.getMonth() + 1) + '/' + dateString.getDate();
            var date = document.createElement("p");
            date.innerText = newdate;

            var img = document.createElement("img");
            img.setAttribute("src", "/fileDirectory/Projects/Images/" + val.img)


            content.appendChild(title);
            content.appendChild(cats);

            card.appendChild(img);
            card.appendChild(content);
            card.appendChild(link);
            card.appendChild(date);


            if (selected.length == 0) {
      
                $(".project-row").append(card);

                //console.log(val);

            } else {
                if (selected.includes(val.id) || selected.length == 0) {
                    $(".project-row").append(card);
                    //console.log(val);
                }
            }
        }
    }
}

function filterProducts(el) {
    
    if (el.value != 0) {
        var val = categories[el.value].project;

        if (!selected.includes(val)) {
            if (val != null) {
                selected.push(val);
            }

        } else {

            selected.pop(val);
        }

        if (categories[el.value].parent == null) {
            renderSubCategories(el.value, categories);
        }

    } else {
        selected = [];
        renderSubCategories(el.value);
    }

    for (var a in currentCats) {
        if (selected.includes(categories[currentCats[a]].project)) {
            $('button[type="button"][value=' + currentCats[a] + ']').addClass("activeFilter");
        } else {
            $('button[type="button"][value=' + currentCats[a] + ']').removeClass("activeFilter");

        }
    }
    //console.log(currentCats);
    console.log(selected);
    renderProjects();
}

function renderCategories() {
    for (var key in categories) {
        if (categories.hasOwnProperty(key)) {
            var val = categories[key];

            if (val.parent == null) {
                if (!currentCats.includes(key)) {
                    currentCats.push(key);
                }
                var filterbtn = document.createElement("BUTTON");
                filterbtn.setAttribute("type", "button");
                filterbtn.setAttribute("class", "filter-btn");
                filterbtn.setAttribute("value", key);
                filterbtn.setAttribute("onclick", "filterProducts(this)");
                filterbtn.innerText = val.name;
                $(".category-row").append(filterbtn);
            }
        }
    }
}


function renderSubCategories(cat) {
    $(".subcategory-row").empty();

    for (var key in categories) {
        if (categories.hasOwnProperty(key)) {
            var val = categories[key];
            if (val.parent == cat && val.project != null && cat != 0) {
                if (!currentCats.includes(key)) {
                    currentCats.push(key);
                }
                var filterbtn = document.createElement("BUTTON");
                filterbtn.setAttribute("type", "button");
                filterbtn.setAttribute("class", "filter-btn");
                filterbtn.setAttribute("value", key);
                filterbtn.setAttribute("onclick", "filterProducts(this)");
                filterbtn.innerText = val.name;
                $(".subcategory-row").append(filterbtn);
            }
        }
    }


}