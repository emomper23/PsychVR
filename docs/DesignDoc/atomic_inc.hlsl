PS_OUTPUT main( PS_INPUT input )
{
    PS_OUTPUT outPix;
	
	uint counterI = BufferIncr.IncrementCounter();
	
	uint counterD = BufferDecr.DecrementCounter();
	
	app.Append(1);
	cons.Consume();
	
	outPix.Colour0 = float4(0.001 * counterI, 
	0.001 * counterD, 1, 1);
	
	return outPix;
}
