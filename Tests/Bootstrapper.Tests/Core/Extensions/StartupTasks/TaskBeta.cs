﻿using Bootstrap.Extensions.StartupTasks;
using Bootstrap.Tests.Extensions.TestImplementations;

namespace Bootstrap.Tests.Core.Extensions.StartupTasks
{
    [Task(DelayStartBy=10)]
    public class TaskBeta : TestStartupTask { }
}