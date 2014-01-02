using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManageAttribute
{

    /// <summary>
    /// Обозначает proxy-поля для языковых структур
    /// </summary>
    public class LangColumnAttribute : Attribute
    {
    }

    /// <summary>
    /// Обозначает первичное поле (обычно ID)
    /// </summary>
    public class PrimaryFieldAttribute : Attribute
    {
    }

    /// <summary>
    ///  В Index Scaffolding выведет в таблице
    /// </summary>
    public class ShowIndexAttribute : Attribute
    {
    }

    /// <summary>
    /// В Edit Scaffolding проставит CheckBox
    /// </summary>
    public class CheckBoxFieldAttribute : Attribute
    {
    }

    /// <summary>
    /// В Edit Scaffolding проставит DropDownList
    /// </summary>
    public class DropDownFieldAttribute : Attribute
    {
    }

    /// <summary>
    /// В Edit Scaffolding проставит Hidden
    /// </summary>
    public class HiddenFieldAttribute : Attribute
    {
    }

    /// <summary>
    /// В Edit Scaffolding проставит TextBox с классом htmltext
    /// </summary>
    public class HtmlTextFieldAttribute : Attribute
    {
    }

    /// <summary>
    ///  В Edit Scaffolding проставит RadioButton
    /// </summary>
    public class RadioFieldAttribute : Attribute
    {
    }



    /// <summary>
    ///  В Edit Scaffolding  проставит TextArea
    /// </summary>
    public class TextAreaFieldAttribute : Attribute
    {
    }

    /// <summary>
    ///  В Edit Scaffolding  проставит TextBox
    /// </summary>
    public class TextBoxFieldAttribute : Attribute
    {
    }
}
