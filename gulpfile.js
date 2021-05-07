const gulp = require('gulp');
const fs = require('fs');
const del = require('del');
const mkdirp = require('mkdirp');
const path = require('path');
const { exec } = require('child_process');
const {clean, restore, build, test, pack, publish, run} = require('gulp-dotnet-cli');
const configuration = process.env.BUILD_CONFIGURATION || 'Debug';
const packageName = 'Xerris.DotNet.Core';
const nugetApiUrl = '-s https://api.nuget.org/v3/index.json';
const targetProject = 'src/Xerris.DotNet.Core/Xerris.DotNet.Core.csproj';
let version = '0.0.0-prerelease';

function _clean() {
    return gulp.src('*.sln', {read: false})
        .pipe(clean());
}

function _restore () {
    return gulp.src('*.sln', {read: false})
        .pipe(restore());
}

function _build() {
    return gulp.src('*.sln', {read: false})
        .pipe(build({configuration: configuration, version: version}));
}

function _test() {
    return gulp.src('**/*Test.csproj', {read: false})
        .pipe(test({logger: `junit;LogFileName=${__dirname}/TestResults/xunit/TestOutput.xml`}))
}

function _distDir() {
    return new Promise((resolve, error) => {
        del(['dist'], {force: true}).then(
            () => { mkdirp('dist', resolve);
            });
    });
}

function _publish() {
    return gulp.src(targetProject, {read: false})
        .pipe(publish({
            configuration: configuration, version: version,
            output: path.join(process.cwd(), 'dist'),
        }));
}

function _package() {
    return gulp.src(targetProject)
        .pipe(pack({
            output: path.join(process.cwd(), 'dist') ,
            version: version
        }));
}

async function _version() {
    console.log('Pushing to NUGET!');
    var jsonString = fs.readFileSync('./version.js');
    var nugetVersion = JSON.parse(jsonString);
    nugetVersion.Patch +=1;
    version = `${nugetVersion.Major}.${nugetVersion.Minor}.${nugetVersion.Patch}-prerelease`
    console.log(`new verion: ${version}`);
    fs.writeFileSync('./version.js',JSON.stringify(nugetVersion));
}

async function _push() {
    console.log(`pushing to nuget for verion: ${version}`);
    var cmd = `dotnet nuget push ./dist/${packageName}.${version}.nupkg`;
    var nugetApi = `-k ${process.env.NUGET_APIKEY}`
    var execCmd = `${cmd} ${nugetApiUrl} ${nugetApi}`
    exec(execCmd).on('exit', () => {
        console.log(`pushed to nuget for verion: ${version}`);
    });
}

exports.Version = gulp.series(_clean, _version);
exports.Build = gulp.series(_clean, _restore, _version, _build);
exports.Test = gulp.series(_clean, _restore, _version, _build, _test);
exports.Default = gulp.series(_clean, _restore, _version, _build, _test);
exports.Publish = gulp.series(_distDir, _clean, _version, _build, _publish);
exports.Package = gulp.series(_distDir, _clean, _version, _build, _publish, _package);
exports.Push = gulp.series(_distDir, _clean, _version, _build, _test, _publish, _package, _push);