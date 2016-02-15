/// <binding />
module.exports = function (grunt) {

    var version = grunt.option("semver");
    if(!version || !/\d+\.\d+\.\d+/.test(version)) 
        grunt.fail.fatal("Version is ether not specified by -semver option or has incorrect format. Correct version format is Major.Minor.Revision, 1.2.3 for example.")

    grunt.initConfig({
        version: version,
        replace: {
          bower: {
              options: {
                patterns: [{
                    match: /("version"\s*:\s*")\d+\.\d+\.\d+(")/,
                    replace: "$1<%=version%>$2"
                }]
              },
              files: [
                  { src: "bower.json", dest: "bower.json" },
                  { src: "package.json", dest: "package.json" }
              ]
          },
          assemblyInfo: {
              options: {
                patterns: [{
                    match: /(let\s*Version\s*=\s*")\d+\.\d+\.\d+("\s*\/\/ Assembly semantic version)/,
                    replace: "$1<%=version%>$2"
                }]
              },
              files: [
                  { src: "src/Angara.Chart/AssemblyInfo.fs", dest: "src/Angara.Chart/AssemblyInfo.fs" },
              ]
          }    
        }
    });

    grunt.loadNpmTasks('grunt-replace');
    
    grunt.registerTask('default', ['replace']);
};
