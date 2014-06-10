﻿var AjaxUrl = {
    // 首页处理
    IndexElectricityPieChart: 'action.ashx?action=indexElectricityPieChart',
    IndexElectricityLineChart: 'action.ashx?action=indexElectricityLineChart',
    QueryPieChart: 'action.ashx?action=queryPieChart',
    ShowQueryLineChart: 'action.ashx?action=showQueryLineChart',
    ExportQueryLineChart: 'action.ashx?action=exportQueryLineChart',
    ShowCompareLineChart: 'action.ashx?action=showCompareLineChart',
    ShowCompareLineChartNew: 'action.ashx?action=showCompareLineChartNew', //new
    RealChart: 'action.ashx?action=realChart',
    DeviceRealChart: 'action.ashx?action=deviceRealChart',
    ExportExcelDataRanking: 'action.ashx?action=exportExcelDataRanking',
    ExportExcelDataRankingNew: 'action.ashx?action=exportExcelDataRankingNew',
   // IndexReportChart: 'method=A247FF928CF85A7722127D4F970A552A1F51592BB1D9A0694297B6DD35F9580A9A97CBF0AE4A3F08&dll=A247FF928CF85A77F2813AEDCFFB056B',
   // IndexEneryChain: 'method=A247FF928CF85A7722127D4F970A552A1F51592BB1D9A06970639E908B1C310CC7F62F8D21546030&dll=A247FF928CF85A77F2813AEDCFFB056B',
    // 能耗查询
    EneryQuery: 'method=A247FF928CF85A7722127D4F970A552AED4867E0A084284FF983A1252EFE56F5F8081160EBF81CC379AA93113F838DED&dll=A247FF928CF85A77F2813AEDCFFB056B',
    RightEneryQueryChart: 'method=A247FF928CF85A7722127D4F970A552AED4867E0A084284F673DFB911E675FFAF374998863A700BFAAE5ABF46D175E5505F368B314545A73&dll=A247FF928CF85A77F2813AEDCFFB056B',
    EneryQueryChart: 'method=A247FF928CF85A7722127D4F970A552AED4867E0A084284F8497DA9EC6E8EC795CA0328F03A705ECCB3BBA913CD16925&dll=A247FF928CF85A77F2813AEDCFFB056B',
    EneryQueryPieChart: 'method=A247FF928CF85A7722127D4F970A552AED4867E0A084284F8497DA9EC6E8EC79C534363F5E38FD08FA1057AF7F30E675&dll=A247FF928CF85A77F2813AEDCFFB056B',
    EneryQueryArea: 'method=A247FF928CF85A7722127D4F970A552AAF83DCDA926EEE2C489D840BA2E0A8608449D3BD4DDC5CE481F9F2C844402ED6B1CC250D36A735B5&dll=A247FF928CF85A77F2813AEDCFFB056B',
    EneryQueryAreaPieChart: 'method=A247FF928CF85A7722127D4F970A552AAF83DCDA926EEE2C489D840BA2E0A8608449D3BD4DDC5CE481F9F2C844402ED6DB31297018C6221E&dll=A247FF928CF85A77F2813AEDCFFB056B',
    EneryQueryAreaChart: 'method=A247FF928CF85A7722127D4F970A552AAF83DCDA926EEE2C489D840BA2E0A8608449D3BD4DDC5CE4B8CFF949E912692F0632149442510CE6&dll=A247FF928CF85A77F2813AEDCFFB056B',
    EneryCompareQueryChart: 'method=A247FF928CF85A7722127D4F970A552AAF83DCDA926EEE2C489D840BA2E0A8600F98CAC1006ECF6CF9296366B7FD719FEBD3FA125EA44BE6&dll=A247FF928CF85A77F2813AEDCFFB056B',
    SameEneryCompareQueryChart: 'method=A247FF928CF85A7722127D4F970A552AAF83DCDA926EEE2CE61D0D574D0596372EDC665E61A3AECDD89E99054DE0D9C699408534D286A622D841FFEC5F133190&dll=A247FF928CF85A77F2813AEDCFFB056B',
    ShowComReportList: 'method=A247FF928CF85A7722127D4F970A552A156FC82743283733B94E8E50A38A453A8953DAB541E0609C&dll=A247FF928CF85A77F2813AEDCFFB056B',
    RealTimeData: 'method=A247FF928CF85A7722127D4F970A552ADBEE2EAA84932EAF1CD6B469AE0A280A9C593C816A18B9DDD99E98F904B49582&dll=A247FF928CF85A77F2813AEDCFFB056B',
    DeviceRealData: 'method=A247FF928CF85A7722127D4F970A552ADBEE2EAA84932EAF1CD6B469AE0A280AE224013D023C0391D98398BFA8D0DE4329D6F0F54A06F708&dll=A247FF928CF85A77F2813AEDCFFB056B',
    ElePriceQuery: 'method=A247FF928CF85A77A0C2169BA08EE952BEC401139157FFFDA583A997B3E340A94A2FF80BCA10882A30F60B6946C6A9C966EC309386322DF4&dll=A247FF928CF85A77F2813AEDCFFB056B',
    ElePriceChartQuery: 'method=A247FF928CF85A77A0C2169BA08EE952BEC401139157FFFDA583A997B3E340A94A2FF80BCA10882A7481879EED4645638B7A467F5BDC6CA4&dll=A247FF928CF85A77F2813AEDCFFB056B',
    ExportElePriceQuery: 'method=A247FF928CF85A77A0C2169BA08EE952BEC401139157FFFDA583A997B3E340A9A973A3ED54C19F258022779C1E3619090C31F2D1E845F7116A289213730397DD&dll=A247FF928CF85A77F2813AEDCFFB056B',
    DeviceCompare: 'method=A247FF928CF85A7722127D4F970A552A47C2BEBBD8377875728B51A466BC52A6F0EB808DBF54EA1A3B4D80336306CF7B1BB81657543E2CD160A284AD4D6A1DC3&dll=A247FF928CF85A77F2813AEDCFFB056B',
    DeviceDateCompare: 'method=A247FF928CF85A7722127D4F970A552A47C2BEBBD8377875728B51A466BC52A6B840872BDA09B3E62CFA847AEF726FEB1E2C7FE82C03A0AE6F06EEC2FBA6A41D&dll=A247FF928CF85A77F2813AEDCFFB056B',
    ShowDataRanking: 'method=A247FF928CF85A7722127D4F970A552A26350F6FA7005A154AA7A0462A3ACF0CA6BFEDDBC37A303549B7E9F8C31E74B126991268069F8209&dll=A247FF928CF85A77F2813AEDCFFB056B',
    ShowDataRankingList: 'method=A247FF928CF85A7722127D4F970A552A26350F6FA7005A154AA7A0462A3ACF0CA6BFEDDBC37A3035BAC28D4AAAFE88A0FA0D8CDEF0546B2F&dll=A247FF928CF85A77F2813AEDCFFB056B',
    ExportDataRankingList: 'method=A247FF928CF85A7722127D4F970A552A26350F6FA7005A15FC0F7F8F69CEBE6F7F214D0D62A9E1E65F79FA8E966D3563E1D648E4E982BAFD&dll=A247FF928CF85A77F2813AEDCFFB056B',
    ExportMainQuery: 'method=A247FF928CF85A7722127D4F970A552AED4867E0A084284FD513DF5C2C5CA280902A00D143AA1AAE5A68CCD44153598EDD289BFE10A69B7C&dll=A247FF928CF85A77F2813AEDCFFB056B',
    ExportMainDevice: 'method=A247FF928CF85A7722127D4F970A552AED4867E0A084284FD513DF5C2C5CA280D257F4596FB2ED33640170B33233236762DF118C78CE033C&dll=A247FF928CF85A77F2813AEDCFFB056B',
    ExportMainArea: 'method=A247FF928CF85A7722127D4F970A552AAF83DCDA926EEE2C55F15D6C960FFD18904EB132FCE291961F9834A69C13891B19930391551EE8A4&dll=A247FF928CF85A77F2813AEDCFFB056B',
    ExportReportList: 'method=A247FF928CF85A7722127D4F970A552AAC7061A5585CD8C7AA017018956A0BA4BAB21F697E07E9762F85409EAE80CCD9&dll=A247FF928CF85A77F2813AEDCFFB056B',
    DeptNameByItemCode: 'method=A247FF928CF85A7722127D4F970A552A99E231D820536C2A0E0C008D65F8A1C29DCA25EF7646237DDFFFDFA09206795B&dll=A247FF928CF85A77F2813AEDCFFB056B',
    // 定额分析
    MaxChart: 'method=A247FF928CF85A7722127D4F970A552A5FCDBA2E0238577D35DD5286483CD69EED1695D8F90D8C391F71FB65151A3E21&dll=A247FF928CF85A77F2813AEDCFFB056B',
    GetRunResult: 'method=A247FF928CF85A7722127D4F970A552A5FCDBA2E0238577D5BDD08340B3B991A04BE7955A65F15B2&dll=A247FF928CF85A77F2813AEDCFFB056B',
    MaxChartGoing: 'method=A247FF928CF85A7722127D4F970A552A5FCDBA2E0238577D35DD5286483CD69E970AED739EDA929F1371DDF585EFE3EC&dll=A247FF928CF85A77F2813AEDCFFB056B'
};

var ReportData = {
    BaseData: { starttime: '', endtime: '', itemcode: '', objectid: '', unit: '',objecttype:'0',type: '' }
};

var ReportCompareData = {
    BaseData: { starttime1: '', endtime1: '', starttime2: '', endtime2: '', itemcode: '', objectid: '', unit: '', objecttype: '0' }
};
var ReportRankingData = {
    BaseData: { starttime: '', endtime: '', itemcode: '', objectid: '', unit: '', order: '', top: '', type: '', unittype: '', objecttype: '0' }
};
var msgModel = {
    starttime:'起始时间',
    endtime:'结束时间',
    itemcode:'分类分项',
    objectid:'分析对象',
    unit:'统计类型',
    objecttype: '', 
    type: ''
};
function AjaxServer() { };
NTS = AjaxServer.prototype = {
    NTSAjax: function (purl, pdata, pcallback) {
        $.ajax({
            type: "POST",
            url: "action.ashx?" + purl + "&times=" + new Date().getTime(),
            data: pdata,
            async: false,
            dataType: "json",
            timeout:10000,
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            success: function (data) {
                pcallback(data);
            }
        });
    }
};

