// Tipue 1.52


// ---------- script properties ----------


var results_location = "resultat.htm";


// ---------- end of script properties ----------


function search_form(tip_Form) {
  
	if (tip_Form.elements['d'].value.length > 0) {
		document.cookie = '_hw_d_hw_=' + escape(tip_Form.elements['d'].value) + '; path=/';
		document.cookie = '_hw_n_hw_=0; path=/';
		window.clipboardData.setData("Text",escape(tip_Form.elements["q"].value));
		window.location = results_location;
	}
}
