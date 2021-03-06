properties {
	$base_dir = resolve-path .
	$build_dir = "$base_dir\build"
	$source_dir = "$base_dir\src"
	$result_dir = "$build_dir\results"
	$global:config = "debug"
	$tag = $(git tag -l --points-at HEAD)
	$revision = @{ $true = "{0:00000}" -f [convert]::ToInt32("0" + $env:APPVEYOR_BUILD_NUMBER, 10); $false = "local" }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
	$suffix = @{ $true = ""; $false = "ci-$revision"}[$tag -ne $NULL -and $revision -ne "local"]
	$commitHash = $(git rev-parse --short HEAD)
	$buildSuffix = @{ $true = "$($suffix)-$($commitHash)"; $false = "$($branch)-$($commitHash)" }[$suffix -ne ""]
    $versionSuffix = @{ $true = "--version-suffix=$($suffix)"; $false = ""}[$suffix -ne ""]
}


task default -depends local
task local -depends compile, test
task ci -depends clean, release, compile, test, pack, publish

task publish {
	echo $source_dir
	exec { dotnet publish $source_dir\StatisticalLearning.Host.Api\StatisticalLearning.Api.Host.csproj -c $config -o $result_dir\services\StatisticalLearningApi }
	exec { npm install $source_dir\StatisticalLearning.Website --prefix $source_dir\StatisticalLearning.Website }
	exec { npm run build-azure --prefix $source_dir\StatisticalLearning.Website }
	exec { dotnet publish $source_dir\StatisticalLearning.Website\StatisticalLearning.Website.csproj -c $config -o $result_dir\services\StatisticalLearningWebsite }
}

task clean {
	rd "$source_dir\artifacts" -recurse -force  -ErrorAction SilentlyContinue | out-null
	rd "$base_dir\build" -recurse -force  -ErrorAction SilentlyContinue | out-null
}

task release {
    $global:config = "release"
}

task compile -depends clean {
	echo "build: Tag is $tag"
	echo "build: Package version suffix is $suffix"
	echo "build: Build version suffix is $buildSuffix" 
	
	exec { dotnet --version }
	exec { dotnet --info }

	exec { msbuild -version }
	
    exec { dotnet build .\StatisticalLearning.sln -c $config --version-suffix=$buildSuffix }
}
 
task pack -depends compile {
	exec { dotnet pack $source_dir\StatisticalLearning\StatisticalLearning.csproj -c $config --no-build $versionSuffix --output $result_dir }
}

task test {
    Push-Location -Path $base_dir\tests\StatisticalLearning.Tests

    try {
        exec { & dotnet test -c $config --no-build --no-restore }
    } finally {
        Pop-Location
    }
}