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
Any combination of parameters work.

    $ NChurn -t 5 -c 3        # take top 5, cut off at level 3 and below.
    $ NChurn -d 24-12-2010    # calculate for 24th of Dec, 2010 up to now.
    $ NChurn -c 2 -r xml      # cut off at 2, report output as XML.
 

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
