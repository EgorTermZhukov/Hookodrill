#ifndef __PeakHold__
#define __PeakHold__


void PeakHold_float(float Current, float LastPeak, out float Output, out float NewPeak)
{
    if (Current > LastPeak)
        NewPeak = Current;
    else
        NewPeak = LastPeak;
    Output = NewPeak;
}
#endif