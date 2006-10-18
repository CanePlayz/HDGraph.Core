addEvent(window, "load", hwTopicOnLoad);
function hwTopicOnLoad(){Nifty("#nsr2","smooth  normal top");
Nifty("#bottom","smooth  normal bottom");
Nifty("div.info-note,div.info-astuce,div.info-alerte,div.info-valide,div.info-info,div.info-avertissement","smooth  normal all");
var vtip_formsubmit=document.getElementById('tip_form'); vtip_formsubmit.onsubmit=f5;
var vdfocus=document.getElementById('d'); vdfocus.onfocus=f6;
}

function f5(){search_form(this);return false;}
function f6(){if (d_first){this.value='';d_first = false;};}


// © John Resig - http://ejohn.org/projects/flexible-javascript-events/

function addEvent( obj, type, fn )
{
	if (obj.addEventListener)
		obj.addEventListener( type, fn, false );
	else if (obj.attachEvent)
	{
		obj["e"+type+fn] = fn;
		obj[type+fn] = function() { obj["e"+type+fn]( window.event ); }
		obj.attachEvent( "on"+type, obj[type+fn] );
	}
}

function removeEvent( obj, type, fn )
{
	if (obj.removeEventListener)
		obj.removeEventListener( type, fn, false );
	else if (obj.detachEvent)
	{
		obj.detachEvent( "on"+type, obj[type+fn] );
		obj[type+fn] = null;
		obj["e"+type+fn] = null;
	}
}
