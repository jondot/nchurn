NChurn
======

NChurn is a utility that helps asses the churn level of your files in your repository.  
Churn can help you detect which files are changed the most in their life time. This helps identify potential bug hives, and improper design.  
The best thing to do is to plug NChurn into your build process and store history of each run. Then, you can plot the evolution of your repository's churn.  


NChurn currently supports file-level churns for

* Git
* SVN
* TFS (Team Foundation)


As outputs, NChurn supports

* Table (plain ASCII)
* CSV
* XML (recommended for builds)


NChurn can also take top # of items to display, cut off churn level, and a date to go back up to.

Getting Started
---------------

	$ NChurn -h
	NChurn 0.1.0.0
	Usage: NChurn
	       NChurn -c 4 -d 24-3-2010 -t 10

	  d, from-date    Past date to calculate churn from, to now.
	  c, churn        Minimal churn rate. Churn results below are cut off.
	  t, top          Return this number of top records.
	  r, report       Type of report to output.
	  help            Dispaly this help screen.

  
Any combination of parameters work.

	$ NChurn -t 5 -c 3        # take top 5, cut off at level 3 and below.
	$ NChurn -d 24-12-2010    # calculate for 24th of Dec, 2010 up to now.
	$ NChurn -c 2 -r xml      # cut off at 2, report output as XML.

Here is a sample of a run, which cuts off at 8, and uses the default table report:

	$ NChurn -c 8
	+--------------------------------------------------+
	| lib/rubikon/application/instance_methods.rb | 48 |
	| lib/rubikon/application.rb                  | 30 |
	| test/test.rb                                | 30 |
	| lib/rubikon/command.rb                      | 28 |
	| lib/rubikon/parameter.rb                    | 17 |
	| test/application_tests.rb                   | 14 |
	| Rakefile                                    | 13 |
	| lib/rubikon/application/dsl_methods.rb      | 12 |
	| README.md                                   | 11 |
	| samples/helloworld/hello_world.rb           | 11 |
	| lib/rubikon.rb                              | 10 |
	| lib/rubikon/exceptions.rb                   | 10 |
	| lib/rubikon/flag.rb                         | 10 |
	| lib/rubikon/action.rb                       | 9  |
	| lib/rubikon/application/base.rb             | 9  |
	| lib/rubikon/option.rb                       | 9  |
	| lib/rubikon/progress_bar.rb                 | 9  |
	| samples/helloworld.rb                       | 9  |
	+--------------------------------------------------+

And here is an example of taking the top 4 records on NChurn's git repo, output as xml report.

	$ NChurn -t 4 -r xml
	<?xml version="1.0" encoding="IBM437"?>
	<NChurnAnalysisResult xmlns:i="http://www.w3.org/2001/XMLSchema-instance" xmlns="http://schemas.datacontract.org/2004/07/NChurn.Core.Reporters">
	  <FileChurns>
	    <FileChurn>
	      <File>README.md</File>
	      <Value>2</Value>
	    </FileChurn>
	    <FileChurn>
	      <File>.gitignore</File>
	      <Value>1</Value>
	    </FileChurn>
	    <FileChurn>
	      <File>AssemblyInfo.cs</File>
	      <Value>1</Value>
	    </FileChurn>
	    <FileChurn>
	      <File>Gemfile</File>
	      <Value>1</Value>
	    </FileChurn>
	  </FileChurns>
	</NChurnAnalysisResult>

Contribute
----------

NChurn is an open-source project. Therefore you are free to help improving it.
There are several ways of contributing to NChurn's development:

* Build apps using NChurn and spread the word.
* Bug and features using the [issue tracker][2].
* Submit patches fixing bugs and implementing new functionality.
* Create an NChurn fork on [GitHub][1] and start hacking. Extra points for using GitHubs pull requests and feature branches.

License
-------

This code is free software; you can redistribute it and/or modify it under the
terms of the Apache License. See LICENSE.txt.

Copyright
---------

Copyright (c) 2011, Dotan Nahum <dotan@paracode.com>


[1]: http://github.com/jondot/nchurn
[2]: http://github.com/jondot/nchurn/issues
