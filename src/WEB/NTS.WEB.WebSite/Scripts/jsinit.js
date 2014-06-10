var NameSpace = new Object();

//全局对象仅仅存在register函数，参数为名称空间全路径。

NameSpace.register = function (nameSpaceName) {
    //使用.切割命名空间名称
    var nsArray = nameSpaceName.split('.');
    var sEval = "";
    var sNS = "";
    //循环命名空间的各个部分依次构建命名空间的各个部分
    for (var i = 0; i < nsArray.length; i++) {
        if (i != 0)
            sNS += ".";

        sNS += nsArray[i];

        sEval += "if (typeof(" + sNS + ")=='undefined')" +
                "{ " + sNS + "=new Object(); }";

        //eval是将参数作为代码来执行的。
        continue;
    }
    if (sEval.length > 0)
        eval(sEval);
}
this.NameSpace.register("nts.web9000.JsLibrary");
