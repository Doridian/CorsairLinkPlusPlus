module.exports = function (grunt) {

    var source_directory = "./src/",
		target_directory = "./bin/Release/";

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        copy: {
            default: {
                files: [{
                    expand: true,
                    cwd: source_directory,
                    src: ['*'],
                    filter: 'isFile',
                    dest: target_directory,
                }]
            }
        },
        uglify: {
            options: {
                beautify: false,
                compress: true,
                warnings: true,
                mangle: true,
                sourceMap: true
            },
            default: {
                files: [{
                    expand: true,
                    cwd: source_directory,
                    src: ['js/**/*.js'],
                    dest: target_directory,
                }],
            }
        },
        less: {
            default: {
                files: [
                    {
                        expand: true,
                        cwd: source_directory,
                        src: 'css/**/*.less',
                        dest: target_directory,
                        ext: '.css'
                    }
                ]
            },
        },
        concurrent: {
            default: ['copy:default', 'uglify:default', 'less:default']
        },
        clean: {
            default: [target_directory],
            postbuild: ['.tmp']
        },
    });

    grunt.loadNpmTasks('grunt-concurrent');
    grunt.loadNpmTasks('grunt-contrib-clean');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-copy');

    grunt.registerTask('default', [
		'clean:default',
		'concurrent:default',
		'clean:postbuild'
    ]);
};