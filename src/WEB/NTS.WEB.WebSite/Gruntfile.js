module.exports = function(grunt) {
	"user strict";
	grunt.initConfig({
		pkg: grunt.file.readJSON('package.json'),
		banner: '/* <%= pkg.name %> - v<%= pkg.version %> - <%= grunt.template.today("yyyy-mm-dd HH:MM:ss") %> by <%= pkg.author %> */ \n',
		dir: {
			src: 'jsrc',
			dest: 'js',
			bak: 'bak/<%= pkg.version %> - <%= grunt.template.today("yyyymmddHHMM") %>'
		},
		cfg: {
			license: false,
			exclusion: /^(test|r\.js|build|output|lib|data|TODO)/,
			paths: {
				jquery: 'empty:',
				artTemplate: 'empty:',
				blockui: 'empty:',
				frameHeight: 'empty:',
				energyCalendar: 'empty:',
				jscrollpane: 'empty:',
				mousewheel: 'empty:',
				common: 'empty:'
			}
		},
		// 清空文件夹
		clean: {
			build: ['<%= dir.dest %>']
		},
		// 备份js
		copy: {
			copyfiles: {
				files: [{
					expand: true, 
					cwd: '<%= dir.src %>',
					src: ['**'],
					dest: '<%= dir.dest %>'
				}]
			},
			backupfiles: {
				files: [{
					expand: true,
					cwd: '<%= dir.src %>',
					src: ['**'],
					dest: '<%= dir.bak %>'
				}]
			}
		},
		// 合并js
		concat: {
			jscrollpane: {
				src: [
					'<%= dir.src %>/lib/jscrollpane/jscrollpane.js',
					'<%= dir.src %>/lib/jscrollpane/mousewheel.js'
				],
				dest: '<%= dir.dest %>/lib/jscrollpane/jscrollpane_concat.js'
			}
		},
		// 压缩js
		uglify: {
			options: {
				banner: '<%= banner %>'
			},
			jscrollpane: {
				src: '<%= dir.dest %>/lib/jscrollpane/jscrollpane_concat.js',
				dest: '<%= dir.dest %>/lib/jscrollpane/jscrollpane.min.js'
			},
			blockui: {
				src: '<%= dir.dest %>/lib/blockUI/blockUI.js',
				dest: '<%= dir.dest %>/lib/blockUI/blockui.min.js'
			}
		},
		requirejs: {
			// 首页js
			indexjs: {
				options: {
					baseUrl: '<%= dir.src %>/view',
					name: 'index_main',
					out: '<%= dir.dest %>/view/index_main.js',
					paths: '<%= cfg.paths %>',
					preserveLicenseComments: '<%= cfg.license %>',
					fileExclusionRegExp: '<%= cfg.exclusion %>'
				}
			},
		}
	});
	grunt.loadNpmTasks('grunt-contrib-clean');
	grunt.loadNpmTasks('grunt-contrib-copy');
	grunt.loadNpmTasks('grunt-contrib-concat');
	grunt.loadNpmTasks('grunt-contrib-uglify');
	grunt.loadNpmTasks('grunt-contrib-requirejs');

	grunt.registerTask('default', ['clean', 'copy', 'concat', 'uglify', 'requirejs']);
};