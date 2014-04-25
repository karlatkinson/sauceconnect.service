require 'rubygems'
require 'bundler/setup'
require 'albacore'
require 'configatron'
require 'rake/clean'

configatron.configure_from_hash(YAML::load(File.open("configatron.env.yml")).to_hash)

ENV['EnableNuGetPackageRestore'] = 'true'

@build_path = File.absolute_path(configatron.dir.build)

CLOBBER.include(configatron.dir.build)

# COMMON TASKS
desc "Builds"
task :default => ['building:build']

# NAMESPACED TASKS

namespace :building do

  desc "Build solution"
  msbuild :build, [:config] => [:clobber] do |msb|
    msb.properties = { :configuration => :Release, :outputPath => @build_path }
    msb.targets = [ :Clean, :Build ]
    msb.other_switches :nodeReuse => false
  end

end

Albacore.configure do |config|

  config.msbuild do |msb|
    msb.command = 'C:/Windows/Microsoft.NET/Framework/v4.0.30319/msbuild.exe'
    msb.solution = configatron.solution
    msb.verbosity = "m"
  end

end