require.config({
    urlArgs: '',
    baseUrl: '../js/',
    paths: {
        jquery: 'lib/jquery-1.11.1.min',
        easing: 'lib/jquery.easing.1.3',
        highcharts: 'lib/highcharts/highcharts',
        highchartsmore: 'lib/highcharts/highcharts-more',
        chartsmod: 'chartsmod',
        mod: 'mod',
        index: 'view/index'
    },
    shim:{
        easing:['jquery'],
        highcharts:['jquery'],
        highchartsmore: ['highcharts']
    }
});

require(['index'], function(Index){
    new Index();
});
