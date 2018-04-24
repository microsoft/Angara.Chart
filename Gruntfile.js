/// <binding />
module.exports = function (grunt) {

    grunt.initConfig({
        copy: {
            chartjs: {
                files: [
                    { src: 'src/Angara.ChartJS/scripts/Chart.js', dest: 'dist/Chart.js'}
                ]
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.registerTask('default', ['copy']);
};