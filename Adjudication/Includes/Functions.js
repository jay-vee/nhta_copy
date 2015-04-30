/*******************************************************
Author: Joe Vago
Date: 5/23/08
Purpose: Function used on page load to set the timeout for a page
*******************************************************/
function SetPageTimeoutForRedirect(sAccessLevel)
{
	ShowAccessNavs(sAccessLevel);
}


/*******************************************************
Author: Joe Vago
Date: 2/10/07
Purpose: Function used on page load to show/hide buttons
*******************************************************/
function ShowAccessNavs(sAccessLevel)
{
	//setup the site's tabs selections
	//	1	Administrator
	//	2	Liaison & Adjudicator
	//	3	Liaison
	//	4	Adjudicator
	//	5	Guest
	
	switch (sAccessLevel) 
	{
	case "1":
			Menu_Adjudicator.style.visibility="visible";
			Menu_Admin.style.visibility="visible";
			Menu_Liaison.style.visibility="visible";
			break;					
	case "2":
			Menu_Adjudicator.style.visibility="visible";
			Menu_Admin.style.visibility="hidden";
			Menu_Admin.style.display = "none";
			Menu_Liaison.style.visibility="visible";
			break;					
	case "3":
			Menu_Adjudicator.style.visibility="hidden";
			Menu_Adjudicator.style.display = "none";
			Menu_Admin.style.visibility="hidden";
			Menu_Admin.style.display = "none";
			Menu_Liaison.style.visibility="visible";
			break;					
	case "4":
			Menu_Adjudicator.style.visibility="visible";
			Menu_Admin.style.visibility="hidden";
			Menu_Admin.style.display = "none";
			Menu_Liaison.style.visibility="hidden";
			Menu_Liaison.style.display = "none";
			break;					
	case "5":
			Menu_Adjudicator.style.visibility="visible";
			Menu_Admin.style.visibility="hidden";
			Menu_Admin.style.display = "none";
			Menu_Liaison.style.visibility="hidden";
			Menu_Liaison.style.display = "none";
			break;					
	default :
			Menu_Adjudicator.style.visibility="hidden";
			Menu_Adjudicator.style.display = "none";
			Menu_Admin.style.visibility="hidden";
			Menu_Admin.style.display = "none";
			Menu_Liaison.style.visibility="hidden";
			Menu_Liaison.style.display = "none";
			break;					
    } 	
    
}