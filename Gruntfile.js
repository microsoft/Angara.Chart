/// <binding />
module.exports = function (grunt) {

    grunt.initConfig({
        bower: {
            install: {
                options: {
                    targetDir: 'src/Angara.ChartJS/lib',
                    layout: 'byType',
                    install: true,
                    verbose: false,
                    cleanTargetDir: false,
                    cleanBowerDir: false,
                    bowerOptions: {}
                }
            }
        },
        copy: {
            chartjs: {
                files: [
                    { src: 'src/Angara.ChartJS/scripts/Chart.js', dest: 'dist/Chart.js'}
                ]
            }
        }
    });

    grunt.loadNpmTasks('grunt-bower-task');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.registerTask('default', ['bower','copy']);
};