
var imageCount = 0;
var fileAdded = false;
var options = document.getElementsByClassName("cat-option");
var initCats = document.getElementsByClassName("initial-cat");
var initCheck = document.getElementsByClassName("init-check");

$(document).ready(function () {
    $('.custom-file-input').on("change", function () {
        var fileLabel = $(this).next('.custom-file-label');
        files = $(this)[0].files;
        if (files.length > 1) {
            fileAdded = true;
            fileLabel.html(files.length + ' files selected');
        } else if (files.length == 1) {
            fileAdded = true;
            fileLabel.html(files[0].name);
        }
    });

    setInitialState();

 
});

function setInitialState() {

    for (var i = 0; i < initCats.length; i++) {
        selected.push(parseInt(document.getElementsByClassName("initial-cat-id")[i].value));
    }

    for (var i = 0; i < initCheck.length; i++) {

        if (selected.includes(initCheck[i].value)) {
            initCheck[i].parentNode.parentElement.style.display = 'none';
        }
        if (initCheck[i].checked == true) {
            var input = document.getElementsByName("index-" + initCheck[i].classList[1])[0];
            //document.getElementById("parent-" + input.value + "35").setAttribute("value", input.id);
            var parent = document.createElement("INPUT");
            parent.setAttribute("type", "hidden");
            parent.setAttribute("value", input.id);
            parent.setAttribute("id", "parent-" + initCheck[i].value);
            parent.setAttribute("name", "pBridges[" + input.value + "+35].Category.ParentId");
            initCheck[i].insertAdjacentElement('afterend', parent);

           
            selected.push(parseInt(initCheck[i].value));
            var checked = initCheck[i];
            var similar = document.getElementsByClassName("check-" + initCheck[i].value);
            for (var x = 0; x < similar.length; x++) {
                if (checked != similar[x]) {
                    similar[x].parentNode.parentElement.style.display = 'none';
                }
            }
        }
        
    }
    for (var i = 0; i < options.length; i++) {
        if (selected.includes(options[i].value)) {
            options[i].style.display = 'none';
        }

    }

    console.log(selected);

}

function addCategory(name) {
    var input = document.getElementsByName(name)[0];
    $.ajax({
        type: "POST",
        url: '/Dashboard/AddCategory?name=' + input.value,
        success: function (data) {
            if (data.success) {
                console.log(data.msg);
                var newOption = document.createElement("OPTION");
                newOption.setAttribute("value", data.id);
                newOption.setAttribute("class", "cat-option");
                newOption.innerText = input.value;
                $('.cat-select').append(newOption);
                input.value = "";

            } else {
                alert(data.msg);
            }
        }
    });


}

function DeleteProject(id, el) {

    var continueAction = confirm('Are you sure about this?');
    console.log("clicked");
    console.log(id);
    if (continueAction == true) {
        $.ajax({
            type: "DELETE",
            url: '/Dashboard/DeleteProject?id=' + id,
            success: function (data) {
                if (data.success) {
                    console.log(data.msg);
                    $(el).parent().parent().hide();

                } else {
                    console.log(data.msg);
                }
            }
        });
    } else {
        return
    }

}

function DeleteImage(id, el) {
    if (fileAdded == false) {
        imageCount = imageCount + 1;
    }
    
    console.log("image delete clicked");
    $.ajax({
        type: "DELETE",
        url: '/Dashboard/DeleteImage?id=' + id + '&imgCount=' + imageCount,
        success: function (data) {
            imageCount = 0;
            if (data.success) {
                console.log(data.msg);
                $(el).parent().hide();
            } else {
                alert(data.msg);
                
            }
        }
    });
}

function selectCategory(id) {
    window.location.href = '/Dashboard/Categories?id=' + id;

}




var selected = [];
function renderSubCatHTML(el, categories, iterator, parents ) {
    var takenChildCat = document.getElementsByClassName("check-" + el.value);
    console.log(takenChildCat);
    for (var i = 0; i < takenChildCat.length; i++) {
        takenChildCat[i].parentNode.parentElement.style.display = 'none';

    }

    console.log(selected);
    var subList = document.createElement("UL");
    subList.setAttribute("class", "list-unstyled subcat-list");
    subList.setAttribute("id", "list-" + el.value);

   
    var label = document.createElement("LABEL");
    label.innerText = "Choose sub categories";
    label.setAttribute("class", "subcat-label");

    subList.appendChild(label);
    

    for (var key in categories) {
        iterator = iterator + 2;
        if (categories.hasOwnProperty(key)) {
            var val = categories[key];
            var indexInput2 = document.createElement("INPUT");
            indexInput2.setAttribute("type", "hidden");
            indexInput2.setAttribute("value", iterator);
            indexInput2.setAttribute("name", "pBridges.Index");
            var tagName = document.createElement("INPUT");
            tagName.setAttribute("type", "hidden");
            tagName.setAttribute("value", val.name);
            tagName.setAttribute("name", "pBridges[" + iterator + "].Category.Name");
            var checkboxLabel = document.createElement("LABEL");
            checkboxLabel.innerText = val.name;
            var parent = document.createElement("INPUT");
            parent.setAttribute("type", "hidden");
            parent.setAttribute("value", "");
            parent.setAttribute("id", "parent-" + key);
            parent.setAttribute("name", "pBridges[" + iterator + "].Category.ParentId");
            var checkboxInput = document.createElement("INPUT");
            checkboxInput.setAttribute("type", "checkbox");
            checkboxInput.setAttribute("value", key);
            checkboxInput.setAttribute("name", "pBridges[" + iterator + "].Category.ID");
            checkboxInput.setAttribute("class", "check-" + key);
            checkboxInput.setAttribute("onclick", "addSubCategoryToList("+el.value+", this, "+ iterator + "," + key + " )");
            var checkboxDiv = document.createElement("DIV");
            checkboxDiv.setAttribute("class", "checkbox");
            var li = document.createElement("LI");
            if ((val.parent == null && !parents.includes(parseInt(key))) || val.parent == el.value) {
                checkboxDiv.appendChild(indexInput2);
                checkboxDiv.appendChild(tagName);
                checkboxDiv.appendChild(checkboxInput);
                checkboxDiv.appendChild(checkboxLabel);
                //checkboxDiv.appendChild(parent);
                li.appendChild(checkboxDiv);
                subList.appendChild(li);
            } 
            
            if (parseInt(key) == el.value) {
                selected.push(parseInt(key));
            }
            if (selected.includes(parseInt(key)) == true && selected.length > 0) {
                li.style.display = "none";
            } 
        }
   
    }

    return subList;

}


function addSubCategoryToList(id, el, i, selfid) {
    var similar = document.getElementsByClassName("check-" + selfid);
    var p = document.getElementById("parent-" + selfid);
    if (el.checked == true) {
        $("#parent-" + selfid).remove();
        var parent = document.createElement("INPUT");
        parent.setAttribute("type", "hidden");
        parent.setAttribute("value", id);
        parent.setAttribute("id", "parent-" + selfid);
        parent.setAttribute("name", "pBridges[" + i + "].Category.ParentId");
        el.insertAdjacentElement('afterend', parent);
        
        for (var i = 0; i < similar.length; i++) {
            if (el != similar[i]) {
                similar[i].parentNode.parentElement.style.display = 'none';
            }
        }
        //for (var i = 0; i < options.length; i++) {
        //    if (selfid == options[i].value) {
        //        options[i].style.display = 'none';
        //    } 

        //}
        selected.push(selfid);
    } else {
        $("#parent-" + selfid).remove();
        var parent = document.createElement("INPUT");
        parent.setAttribute("type", "hidden");
        parent.setAttribute("value", "");
        parent.setAttribute("id", "parent-" + selfid);
        parent.setAttribute("name", "pBridges[" + i + "].Category.ParentId");
        el.insertAdjacentElement('afterend', parent);
        
        for (var i = 0; i < similar.length; i++) {
            if (el != similar[i]) {
                similar[i].parentNode.parentElement.style.display = 'block';
            }
        }
        selected.pop(selfid);
    }

    console.log(selected);
    for (var i = 0; i < options.length; i++) {
        if (selected.includes(options[i].value)) {
            options[i].style.display = 'none';
        } else {
            options[i].style.display = 'block';

        }

    }

}

var iterator = 0;
function selectProjectCategory(el) {
    var id = parseInt(el.id);
    var parents = [];

    iterator = iterator + 254; 
    $.ajax({
        type: "GET",
        url: '/Dashboard/GetCategories?id=' + id,
        success: function (data) {

            if (data.success) {
                var categories = JSON.parse(data.categories);
                parents = data.p;
                console.log(categories);
                console.log(parents);
                var content = categories[el.value];

                var inputWrapper = document.createElement("DIV");
                inputWrapper.setAttribute("class", "input-wrapper");
                inputWrapper.setAttribute("id", "wrapper-" + el.value);

                var indexInput = document.createElement("INPUT");
                indexInput.setAttribute("type", "hidden");
                indexInput.setAttribute("value", iterator);
                indexInput.setAttribute("name", "pBridges.Index");

                 var idInput = document.createElement("INPUT");
                idInput.setAttribute("type", "hidden");
                idInput.setAttribute("value", el.value);
                idInput.setAttribute("name", "pBridges[" + iterator + "].Category.ID");


                var childInput = document.createElement("INPUT");
                childInput.setAttribute("type", "text");
                childInput.setAttribute("value", content.name);
                childInput.setAttribute("readonly", "readonly");
                childInput.setAttribute("name", "pBridges[" + iterator + "].Category.Name");
                childInput.setAttribute("placeholder", content.name);
                childInput.setAttribute("class", "form-control");

                var btn = document.createElement("BUTTON");
                btn.setAttribute("type", "button");
                btn.setAttribute("onclick", "DeleteChild(this, " + el.value + ", \"" + content.name + "\"," + true + ")");
                btn.setAttribute("class", "btn btn-danger btn-sm");
                btn.innerText = "Remove";

                inputWrapper.appendChild(childInput);
                inputWrapper.appendChild(btn);
                inputWrapper.append(idInput);
                inputWrapper.append(indexInput);

                var sublist = renderSubCatHTML(el, categories, iterator, parents);


                var projectDiv = $(".project-cats");
               
                projectDiv.append(inputWrapper);
                projectDiv.append(sublist);



            } else {
                console.log(data.msg);
            }
        }
    });

    //child.setAttribute("class", "project-cat");
    //var indexInput = document.createElement("INPUT");
    //indexInput.setAttribute("type", "hidden");
    //indexInput.setAttribute("value", iterator);
    //indexInput.setAttribute("name", "categories.Index");

    //var parentIdInput = document.createElement("INPUT");
    //parentIdInput.setAttribute("type", "hidden");
    //parentIdInput.setAttribute("value", parentId);
    //parentIdInput.setAttribute("name", "categories[" + iterator + "].ParentId");

    //var projectIdInput = document.createElement("INPUT");
    //projectIdInput.setAttribute("type", "hidden");
    //projectIdInput.setAttribute("value", projectId);
    //projectIdInput.setAttribute("name", "categories[" + iterator + "].ProjectID");

    //var idInput = document.createElement("INPUT");
    //idInput.setAttribute("type", "hidden");
    //idInput.setAttribute("value", val);
    //idInput.setAttribute("name", "categories[" + iterator + "].ID");

    //var childInput = document.createElement("INPUT");
    //childInput.setAttribute("type", "text");
    //childInput.setAttribute("value", content);
    //childInput.setAttribute("readonly", "readonly");
    //childInput.setAttribute("name", "categories[" + iterator + "].Name");
    //childInput.setAttribute("placeholder", content);
    //childInput.setAttribute("class", "form-control");

    //var btn = document.createElement("BUTTON");
    //btn.setAttribute("type", "button");
    //btn.setAttribute("onclick", "DeleteChild(this, " + val + ", \"" + content + "\"," + true + ")");
    //btn.setAttribute("class", "btn btn-danger btn-sm");
    //btn.innerText = "Delete";

    //child.append(indexInput);
    //child.append(parentIdInput);
    //child.append(projectIdInput);
    //child.append(idInput);
    //child.append(childInput);
    //child.append(btn);

}



var iterator = 0;
function selectSubCategory(el) {

    iterator = iterator + 254;
    var val = el.value;
    var content = el.options[el.selectedIndex].text;
    var child = document.createElement("DIV");


    $.ajax({
        type: "GET",
        url: '/Dashboard/GetCategory?id=' + val,
        success: function (data) {
            if (data.success) {
                console.log(data.msg);
                var parentId = data.parentId;
                var projectId = data.projectId
                console.log(parentId);
                console.log(projectId);

                child.setAttribute("class", "project-cat");
                var indexInput = document.createElement("INPUT");
                indexInput.setAttribute("type", "hidden");
                indexInput.setAttribute("value", iterator);
                indexInput.setAttribute("name", "categories.Index");

                var parentIdInput = document.createElement("INPUT");
                parentIdInput.setAttribute("type", "hidden");
                parentIdInput.setAttribute("value", parentId);
                parentIdInput.setAttribute("name", "categories[" + iterator + "].ParentId");

                var projectIdInput = document.createElement("INPUT");
                projectIdInput.setAttribute("type", "hidden");
                projectIdInput.setAttribute("value", projectId);
                projectIdInput.setAttribute("name", "categories[" + iterator + "].ProjectID");

                var idInput = document.createElement("INPUT");
                idInput.setAttribute("type", "hidden");
                idInput.setAttribute("value", val);
                idInput.setAttribute("name", "categories[" + iterator + "].ID");

                var childInput = document.createElement("INPUT");
                childInput.setAttribute("type", "text");
                childInput.setAttribute("value", content);
                childInput.setAttribute("readonly", "readonly");
                childInput.setAttribute("name", "categories[" + iterator + "].Name");
                childInput.setAttribute("placeholder", content);
                childInput.setAttribute("class", "form-control");


                var btn = document.createElement("BUTTON");
                var span = document.createElement("SPAN");
                span.innerHTML = "x";
                btn.setAttribute("type", "button");
                btn.setAttribute("onclick", "DeleteChild(this, " + val + ", \"" + content + "\"," + true + ")");
                btn.setAttribute("class", "close subcat-close");
                btn.appendChild(span);

                child.append(indexInput);
                child.append(parentIdInput);
                child.append(projectIdInput);
                child.append(idInput);
                child.append(childInput);
                child.append(btn);
                $(".project-cats").append(child);
            } else {
                console.log(data.msg);

            }
        }
    });
}



var iter = 0;
function selectChildCategory(el) {
    iter = iter + 154;
    var val = el.value;
    var content = el.options[el.selectedIndex].text;

    $.ajax({
        type: "GET",
        url: '/Dashboard/GetCategory?id=' + val,
        success: function (data) {
            if (data.success) {
                console.log(data.msg);
                var parentId = data.parentId;
                var projectId = data.projectId
                console.log(parentId);
                console.log(projectId);
                var child = document.createElement("DIV");
                child.setAttribute("class", "child");
                var indexInput = document.createElement("INPUT");
                indexInput.setAttribute("type", "hidden");
                indexInput.setAttribute("value", iter);
                indexInput.setAttribute("name", "categories.Index");

                var parentIdInput = document.createElement("INPUT");
                parentIdInput.setAttribute("type", "hidden");
                parentIdInput.setAttribute("value", parentId);
                parentIdInput.setAttribute("name", "categories[" + iterator + "].ParentId");

                var projectIdInput = document.createElement("INPUT");
                projectIdInput.setAttribute("type", "hidden");
                projectIdInput.setAttribute("value", projectId);
                projectIdInput.setAttribute("name", "categories[" + iterator + "].ProjectID");


                var idInput = document.createElement("INPUT");
                idInput.setAttribute("type", "hidden");
                idInput.setAttribute("value", val);
                idInput.setAttribute("name", "categories[" + iter + "].ID");

                var childInput = document.createElement("INPUT");
                childInput.setAttribute("type", "text");
                childInput.setAttribute("value", content);
                childInput.setAttribute("readonly", "readonly");
                childInput.setAttribute("name", "categories[" + iter + "].Name");
                childInput.setAttribute("placeholder", content);
                childInput.setAttribute("class", "form-control");

                var btn = document.createElement("BUTTON");
                btn.setAttribute("type", "button");
                btn.setAttribute("onclick", "DeleteChild(this, " + val + ", \"" + content + "\"," + true + ")");
                btn.setAttribute("class", "btn btn-danger btn-sm");
                btn.innerText = "Remove";

                child.append(indexInput);
                child.append(parentIdInput);
                child.append(projectIdInput);
                child.append(idInput);
                child.append(childInput);
                child.append(btn);
                $("#childCats").append(child);

            } else {
                console.log(data.msg);

            }
        }
    });




}

function DeleteCategory(id, el) {
    $.ajax({
        type: "DELETE",
        url: '/Dashboard/DeleteCategory?id=' + id,
        success: function (data) {
            if (data.success) {
                $(el).parent().parent().hide();
                console.log(data.msg);
            } else {
                console.log(data.msg);
            }
        }
    });

}

function DeleteChild(el, id, name, sub) {
    var thisLI = document.getElementById("list-" + id);
    var thisWrapper = document.getElementById("wrapper-" + id);
    thisLI.remove();
    thisWrapper.remove();
    //$(el).parent().remove();
    //$.ajax({
    //    type: "DELETE",
    //    url: '/Dashboard/DeleteCategory?id=' + id + '&subcategory=' + sub,
    //    success: function (data) {
    //        if (data.success) {
    //            console.log(data.msg);
    //            var sel = document.createElement("OPTION");
    //            sel.setAttribute("value", id);
    //            sel.innerText = name;
                
    //            $('.child-select').append(sel);
    //        } else {
    //            console.log(data.msg);

    //        }
    //    }
    //});
    //if (id != undefined || name != undefined) {
    //    $('.child-select').append(sel);
    //}
}

