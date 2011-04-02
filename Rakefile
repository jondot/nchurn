require "rubygems"
require "bundler"
Bundler.setup

require 'albacore'
require 'version_bumper'



desc "Build"
msbuild :build => :assemblyinfo do |msb|
  msb.properties :configuration => :Release
  msb.targets :Clean, :Build
  msb.solution = "NChurn.sln"
end

output :output => [:test, :merge, :churn] do |out|
	out.from '.'
	out.to 'out'
	out.file 'NChurn/bin/release/NChurn.exe', :as=>'NChurn.exe'
	out.file 'LICENSE.txt'
	out.file 'README.md'
	out.file 'VERSION'
	# output can also build a nuspec :) 
	out.erb 'build/nchurn.nuspec.erb', :as => 'nchurn.nuspec', :locals => { :version => bumper_version }
end

zip :zip => :output do | zip |
    zip.directories_to_zip "out"
    zip.output_file = "nchurn.v#{bumper_version.to_s}.zip"
    zip.output_path = File.dirname(__FILE__)
end

task :nu => :output do
	`tools/nuget/nuget.exe p out/nchurn.nuspec`	
end

task :deploy => [:zip, :nu] do
end

desc "Test"
nunit :test => :build do |nunit|
	nunit.command = "Tools/NUnit/nunit-console.exe"
	#  nunit.options '/framework v4.0.30319'

	nunit.assemblies "NChurn.Core.Tests/bin/release/NChurn.Core.Tests.dll"
end

exec :merge do |cmd|
	cmd.command = 'tools\ilmerge\ilmerge.exe'
	#  cmd.parameters ='/targetplatform:v4,C:\Windows\Microsoft.NET\Framework\v4.0.30319 /out:NChurn\bin\release\NChurn.exe NChurn\bin\release
	cmd.parameters ='/out:NChurn\bin\release\NChurn.exe NChurn\bin\release\NChurn.CLI.exe NChurn\bin\release\NChurn.Core.dll NChurn\bin\release\CommandLine.dll'
	puts 'merged.'
end

assemblyinfo :assemblyinfo do |asm|
  asm.version = bumper_version.to_s
  asm.file_version = bumper_version.to_s
  
  asm.company_name = "Paracode"
  asm.product_name = "NChurn"
  asm.copyright = "Paracode (c) 2010-2011"
  asm.output_file = "AssemblyInfo.cs"
end


desc "Run a sample NCover Console code coverage"
ncoverconsole :coverage do |ncc|
  ncc.command = "Tools/NCover/NCover.Console.exe"
  ncc.output :xml => "test-coverage.xml"
  ncc.cover_assemblies 'NChurn.Core'

		
  nunit = NUnitTestRunner.new("Tools/NUnit/nunit-console.exe")
  #  nunit.options '/framework=4.0.30319', '/noshadow'
  nunit.options '/noshadow'
  nunit.assemblies "NChurn.Core.Tests/bin/debug/NChurn.Core.Tests.dll"
		
  ncc.testrunner = nunit
end

desc "churn"
nchurn :churn do |nc|
  nc.command = "tools/nchurn/nchurn.exe"
  nc.working_directory = "."
  # here are some examples: 
  # nc.input "file.txt"
  # nc.from DateTime.now #3-12-2010 # Chronic.parse('3 days ago')
  # nc.churn 4
  # nc.churn_precent 30
  # nc.top 10
   nc.report_as :xml
  # nc.env_path 'c:/tools'
  # nc.adapter :git
  # nc.exclude "exe"
  # nc.include "foo"
  nc.output "out/churn-#{bumper_version.to_s}.xml"
end
