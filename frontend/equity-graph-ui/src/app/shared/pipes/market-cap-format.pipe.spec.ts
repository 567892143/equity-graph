import { MarketCapFormatPipe } from './market-cap-format.pipe';

describe('MarketCapFormatPipe', () => {
  const pipe = new MarketCapFormatPipe();

  it('create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  it('formats raw rupees to Crores with Indian digit grouping', () => {
    expect(pipe.transform(160000000000)).toBe('₹16,000 Cr');
    expect(pipe.transform(145000000000)).toBe('₹14,500 Cr');
    expect(pipe.transform(1500000000)).toBe('₹150 Cr');
  });

  it('handles null and undefined', () => {
    expect(pipe.transform(null)).toBe('—');
    expect(pipe.transform(undefined)).toBe('—');
  });
});
