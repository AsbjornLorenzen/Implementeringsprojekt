import numpy as np
import matplotlib.pyplot as plt
import json


for filenr,file in enumerate(['estimates1.json','estimates2.json','estimates3.json']):
    filenr = filenr + 1
    print("results from:",file)
    
    #open json and get results
    f = open(file)
    results = json.load(f)
    t = results['t']
    print("where t = ", t)
    trueSum = results['trueSum']
    array = np.array(results['estimates'])
    sortedArray = np.sort(array)

    #find medians (from non-sorted array)
    medians = np.zeros(9)
    for i in range(9):
        medians[i] = np.median(array[i*11:(i+1)*11])
    #sort medians for plot
    sortedMedians = np.sort(medians)

    #print stats
    mse = 0
    for element in array:
        mse += (element-trueSum)**2/array.shape[0]
    print("mse:",mse)
    print("the variance of the estimator: ",2*trueSum**2/(2**t))
    print("average time in ms: ", np.mean(results['times']))


    #plot params
    size = (6,4)
    ymin = [13200000,13190000,13150000]
    ymax = [13240000,13260000,13350000]
    lin = np.linspace(1,array.shape[0],array.shape[0])
    
    #plot 1: estimates
    fig, ax = plt.subplots(figsize=size)
    A = ax.scatter(lin,sortedArray,label='estimates')
    B, = ax.plot([lin[0],lin[-1]],[trueSum,trueSum], label='true squared sum')
    ax.legend(handles=[A,B])
    ax.set_xlabel('estimate index')
    ax.set_ybound(ymin[filenr-1],ymax[filenr-1])
    ax.set_ylabel('squared sum')
    title = "Estimates, when t = "+str(t)
    ax.set_title(title)
    ax.grid()
    figname = str(filenr)+"_t"+str(t)+"estimates"
    plt.savefig(figname)

    #plot 1: medians
    fig2, ax2 = plt.subplots(figsize=size)
    A2 = ax2.scatter(lin[0:9],sortedMedians,label='medians of estimates')
    B2, = ax2.plot([lin[0],lin[8]],[trueSum,trueSum], label='true squared sum')
    ax2.legend(handles=[A2,B2])
    ax2.set_xlabel('estimate number')
    ax2.set_ybound(ymin[filenr-1],ymax[filenr-1])
    title = "the 10 medians, when t = "+str(t)
    ax2.set_ylabel('squared sum')
    ax2.set_title(title)
    ax2.grid()
    figname = str(filenr)+"_t"+str(t)+"medians"
    plt.savefig(figname)
    
    print("   ")
    print("   ")
