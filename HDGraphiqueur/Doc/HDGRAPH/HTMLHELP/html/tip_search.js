// Tipue 1.52


// ---------- script properties ----------


var results_location = "resultat.htm";
var return_results = 10;
var include_num = 0;
var bold_query = 0;
var include_url = 0;
var cookie_data = '_hw_d_hw_=';
var cookie_ndata = '_hw_n_hw_=';


// ---------- end of script properties ----------


var cookies = document.cookie;
var p = cookies.indexOf(cookie_data);
var pn = cookies.indexOf(cookie_ndata);
var d  = "";var n  = "0";if (p==-1) {d = window.clipboardData.getData("Text");}

if (p != -1) {
	var st = p + cookie_data.length;
	var en = cookies.indexOf(';', st);
	if (en == -1) {
		en = cookies.length;
	}
	var d = cookies.substring(st, en);
	d = unescape(d);
}
if (pn != -1) {
	var st = pn + cookie_ndata.length;
	var en = cookies.indexOf(';', st);
	if (en == -1) {
		en = cookies.length;
	}
	var n = cookies.substring(st, en);
}

var od = d;
var nr = return_results;
n = parseInt(n);
var nb = n + nr;
var nc = 0;
var nd = 0;
var r = new Array();
var co = 0;
var m = 0;

if (d.charAt(0) == '"' && d.charAt(d.length - 1) == '"') {
	m = 1;
}
var rn = d.search(/ or /i);
if (rn >= 0) {
	m = 2;
}

if (m == 0) {
	var woin = new Array();
	d = d.replace(/ and /gi, ' ');
	var w = d.split(' ');
	for (var a = 0; a < w.length; a++) {
		woin[a] = 0;
		if (w[a].charAt(0) == '-') {
			woin[a] = 1;
		}
	}
	for (var a = 0; a < w.length; a++) {
		w[a] = w[a].replace(/^\-|^\+/gi, '');
	}
	a = 0;
	for (var c = 0; c < s.length; c++) {
		var pa = 0;
		var nh = 0;
		for (var i = 0; i < woin.length; i++) {
			if (woin[i] == 0) {
				nh++;
				var pat = new RegExp(w[i], 'i');
				rn = s[c].search(pat);
				if (rn >= 0) {
					pa++;
				} else {
					pa = 0;
				}
			}
			if (woin[i] == 1) {
				var pat = new RegExp(w[i], 'i');
				var rn = s[c].search(pat);
				if (rn >= 0) {
					pa = 0;
				}
			}
		}
		if (pa == nh) {
			r[a] = s[c];
			a++;
		}
	}
	co = a;
}

if (m == 1) {
	d = d.replace(/"/gi, '');
	var a = 0;
	var pat = new RegExp(d, 'i');
	for (var c = 0; c < s.length; c++) {
		var rn = s[c].search(pat);
		if (rn >= 0) {
			r[a] = s[c];
			a++;
		}
	}
	co = a;
}

if (m == 2) {
	d = d.replace(/ or /gi, ' ');
	var w = d.split(' ');
	var a = 0;
	for (var i = 0; i < w.length; i++) {
		var pat = new RegExp(w[i], 'i');
		for (var c = 0; c < s.length; c++) {
		var rn = s[c].search(pat);
			if (rn >= 0) {
				var pa = 0;
				for (var e = 0; e < r.length; e++) {
					if (s[c] == r[e]) {
						pa = 1;
					}
				}
				if (pa == 0) {
					r[a] = s[c];
					a++;
					co++;
				}
			}
		}
	}
}

function write_cookie(nw) {
	document.cookie = '_hw_d_hw_=' + escape(od) + '; path=/';
	document.cookie = '_hw_n_hw_=' + nw + '; path=/';
}


// ---------- External references ----------


var tip_Num = co;

function tip_query() {
	document.tip_Form.d.value = od;
}

function tip_num() {
	document.write(co);
}

function tip_out() {
	if (co == 0) {
    // modified by help&web.com
    // document.write('Your search did not match any documents.<p>Make sure all     //keywords are spelled correctly.<br>Try different or more general keywords.');	
		document.write('');
		return;
	}
	if (n + nr > co) {
		nd = co;	
	} else {
		nd = n + nr;
	}
	for (var a = n; a < nd; a++) {
		var os = r[a].split('^');
		if (bold_query == 1 && m == 0) {
			for (var i = 0; i < w.length; i++) {
				var lw = w[i].length;
				var tw = new RegExp(w[i], 'i');
				rn = os[2].search(tw);
				if (rn >= 0) {
					var o1 = os[2].slice(0, rn);
					var o2 = os[2].slice(rn, rn + lw);
					var o3 = os[2].slice(rn + lw);
					os[2] = o1 + '<b>' + o2 + '</b>' + o3; 
				}
			}
		}
		if (bold_query == 1 && m == 1) {
			var lw = d.length;
			var tw = new RegExp(d, 'i');
			rn = os[2].search(tw);
			if (rn >= 0) {
				var o1 = os[2].slice(0, rn);
				var o2 = os[2].slice(rn, rn + lw);
				var o3 = os[2].slice(rn + lw);
				os[2] = o1 + '<b>' + o2 + '</b>' + o3; 
			}
		}
		if (include_num == 1) {
			if (os[4] == '1') {
		
			document.write(a + 1, '. <a href="', os[1], '" target="_blank">', os[0], '</a>');
    
			} else {
			  document.write(a + 1, '. <a href="', os[1], '">', os[0], '</a>');
				
			}
				if (os[2].length > 1) {
			document.write('<br>', os[2]);
			}			
		} else {
			if (os[4] == '1') {
			   	// modified by help&web
				//document.write('<a href="', os[1], '" target="_blank">', os[0], '</a>');
				if (a%2==0){
				document.write('<div class="search-titre-result"><a href="', os[1], '" target="_blank">', os[0], '</a></div>');
				} else {document.write('<div class="search-titre-result-odd"><a href="', os[1], '" target="_blank">', os[0], '</a></div>');}
			} else {
			  	// modified by help&web
				//document.write('<a href="', os[1], '">', os[0], '</a>');
				if (a%2==0){
				document.write('<div class="search-titre-result"><a href="', os[1], '">', os[0], '</a></div>');
				} else {document.write('<div class="search-titre-result-odd"><a href="', os[1], '">', os[0], '</a></div>');}
			}
			if (os[2].length > 1) {
			 // modified by help&web
			//	document.write('<br>', os[2]);
			if (a%2==0) {
			document.write('<div class="search-result">', os[2]);
			} else {document.write('<div class="search-result-odd">', os[2]);}
			}
		}
		if (include_url == 1) {
			if (os[4] == '1') {
				document.write('<br><a href="', os[1], '" target="_blank">', os[1], '</a><p>');
			} else {
				document.write('<br><a href="', os[1], '">', os[1], '</a><p>');
			}
		} else {
		  // modified by help&web
			//document.write('<p>');
			document.write('</div>');
		}
	}
	if (co > nr) {

		nc = co - nb;
		if (nc > nr) {
			nc = nr;
		}
		document.write('<p>');
	}
	if (n > 1) {
		document.write('<a href="', results_location, '" onclick="write_cookie(', n - nr, ')">Précédent ', nr, '</a> &nbsp;');
	}
	if (nc > 0) {
		document.write('<a href="', results_location, '" onclick="write_cookie(', n + nr, ')">Suivant ', nc, '</a>');
	}
	
}
