window.onload= resizeSplitWndw;
window.onresize= resizeSplitWndw;
window.onbeforeprint= set_to_print;
window.onafterprint= reset_form;

function resizeSplitWndw(){

var onsr= document.all.item("nsr");
var omainbody= document.all.item("mainbody");
var onsr3= document.all.item("nsr3");
var hnsr3=0;

if (onsr3 != null) {if (document.all.nsr3.offsetHeight<12) {hnsr3= 5;} else {hnsr3 = document.all.nsr3.offsetHeight-5;}}

//if (omainbody ==null) return;
if (onsr != null){
document.all.mainbody.style.overflow= "auto";
document.all.nsr.style.width= document.body.offsetWidth;
if (onsr3 != null) {document.all.nsr3.style.width= document.body.offsetWidth-4;}
document.all.mainbody.style.width= document.body.offsetWidth-4;
document.all.mainbody.style.top= document.all.nsr.offsetHeight;
if (onsr3 != null) {document.all.nsr3.style.top= document.body.offsetHeight - hnsr3;}
if (document.body.offsetHeight > document.all.nsr.offsetHeight)
document.all.mainbody.style.height= document.body.offsetHeight - document.all.nsr.offsetHeight- hnsr3;
else document.all.mainbody.style.height=0;
} 
 else {document.getElementById("nsr").style.overflow="fixe";
 document.getElementById("mainbody").style.overflow= "auto";
 document.getElementById("mainbody").style.width= document.body.offsetWidth-4;
 document.getElementById("mainbody").style.top= document.getElementById("nsr").offsetHeight;
if (document.body.offsetHeight > document.getElementById("nsr").offsetHeight)
document.getElementById("mainbody").style.height= document.body.offsetHeight - document.getElementById("nsr").offsetHeight;
else document.getElementById("mainbody").style.height=0;
}
}

function set_to_print(){

var i;
if (window.mainbody)document.all.mainbody.style.height = "auto";

for (i=0; i < document.all.length; i++){
if (document.all[i].tagName == "BODY") {
document.all[i].scroll = "auto";
}
if (document.all[i].tagName == "A") {
document.all[i].outerHTML = "<a href=''>" + document.all[i].innerHTML + "</a>";
}
}
}

function reset_form(){

document.location.reload();
}