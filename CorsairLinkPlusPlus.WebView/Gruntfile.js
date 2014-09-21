/**
 * CorsairLinkPlusPlus
 * Copyright (c) 2014, Mark Dietzer & Simon Schick, All rights reserved.
 *
 * CorsairLinkPlusPlus is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * CorsairLinkPlusPlus is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with CorsairLinkPlusPlus.
 */
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