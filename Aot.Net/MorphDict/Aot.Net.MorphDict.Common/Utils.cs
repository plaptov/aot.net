namespace Aot.Net.MorphDict.Common
{
	public class Utils
	{
		[Flags]
		private enum Letter : ushort
		{
			None = 0,
			fWordDelim = 1,
			RusUpper = 2,
			RusLower = 4,
			GerUpper = 8,
			GerLower = 16,
			EngUpper = 32,
			EngLower = 64,
			OpnBrck = 128,
			ClsBrck = 256,
			UpRomDigits = 512,
			LwRomDigits = 1024,
			LatinVowel = 2048,
			RussianVowel = 4096,
			URL_CHAR = 8192,
		}

		private static readonly Letter[] ASCII = new[]
		{
/*null (nul)*/                                        Letter.fWordDelim,
/*start of heading (soh)*/                            Letter.fWordDelim,
/*start of text (stx)*/                               Letter.fWordDelim,
/*end of text (etx)*/                                 Letter.fWordDelim,
/*end of transmission (eot)*/                         Letter.fWordDelim,
/*enquiry (enq)*/                                     Letter.fWordDelim,
/*acknowledge (ack)*/                                 Letter.fWordDelim,
/*bell (bel)*/                                        Letter.fWordDelim,
/*backspace (bs)*/                                    Letter.fWordDelim,
/*character tabulation (ht)*/                         Letter.fWordDelim,
/*line feed (lf)*/                                    Letter.fWordDelim,
/*line tabulation (vt)*/                              Letter.fWordDelim,
/*form feed (ff)*/                                    Letter.fWordDelim,
/*carriage return (cr)*/                              Letter.fWordDelim,
/*shift out (so)*/                                    Letter.fWordDelim,
/*shift in (si)*/                                     Letter.fWordDelim,
/*datalink escape (dle)*/                             Letter.fWordDelim,
/*device control one (dc1)*/                          Letter.fWordDelim,
/*device control two (dc2)*/                          Letter.fWordDelim,
/*device control three (dc3)*/                        Letter.fWordDelim,
/*device control four (dc4)*/                         Letter.fWordDelim,
/*negative acknowledge (nak)*/                        Letter.fWordDelim,
/*syncronous idle (syn)*/                             Letter.fWordDelim,
/*end of transmission block (etb)*/                   Letter.fWordDelim,
/*cancel (can)*/                                      Letter.fWordDelim,
/*end of medium (em)*/                                Letter.fWordDelim,
/*substitute (sub)*/                                  Letter.fWordDelim,
/*escape (esc)*/                                      Letter.fWordDelim,
/*file separator (is4)*/                              Letter.fWordDelim,
/*group separator (is3)*/                             Letter.fWordDelim,
/*record separator (is2)*/                            Letter.fWordDelim,
/*unit separator (is1)*/                              Letter.fWordDelim,
/*space ' '*/                                         Letter.fWordDelim,
/*exclamation mark '!'*/                              Letter.fWordDelim|Letter.URL_CHAR,
/*quotation mark '"'*/                                Letter.fWordDelim,
/*number sign '#'*/                                   Letter.fWordDelim,
/*dollar sign '$'*/                                   Letter.fWordDelim|Letter.URL_CHAR,
/*percent sign '%'*/                                  Letter.fWordDelim|Letter.URL_CHAR,
/*ampersand '&'*/                                     Letter.fWordDelim|Letter.URL_CHAR,
/*apostrophe '''*/                                    Letter.fWordDelim|Letter.RusUpper|Letter.RusLower, // ������� ���� � ���� ���������
/*left parenthesis '('*/                              Letter.fWordDelim|Letter.OpnBrck|Letter.URL_CHAR,
/*right parenthesis ')'*/                             Letter.fWordDelim|Letter.ClsBrck|Letter.URL_CHAR,
/*asterisk '*'*/                                      Letter.fWordDelim|Letter.URL_CHAR,
/*plus sign '+'*/                                     Letter.fWordDelim|Letter.URL_CHAR,
/*comma ','*/                                         Letter.fWordDelim|Letter.URL_CHAR,
/*hyphen-minus '-'*/                                  Letter.fWordDelim|Letter.URL_CHAR,
/*full stop '.'*/                                     Letter.fWordDelim|Letter.URL_CHAR,
/*solidus '/'*/                                       Letter.fWordDelim|Letter.URL_CHAR,
/*digit zero '0'*/                                    Letter.URL_CHAR,
/*digit one '1'*/                                     Letter.URL_CHAR,
/*digit two '2'*/                                     Letter.URL_CHAR,
/*digit three '3'*/                                   Letter.URL_CHAR,
/*digit four '4'*/                                    Letter.URL_CHAR,
/*digit five '5'*/                                    Letter.URL_CHAR,
/*digit six '6'*/                                     Letter.URL_CHAR,
/*digit seven '7'*/                                   Letter.URL_CHAR,
/*digit eight '8'*/                                   Letter.URL_CHAR,
/*digit nine '9'*/                                    Letter.URL_CHAR,
/*colon ':'*/                                         Letter.fWordDelim|Letter.URL_CHAR,
/*semicolon ';'*/                                     Letter.fWordDelim|Letter.URL_CHAR,
/*less-than sign '<'*/                                Letter.fWordDelim|Letter.OpnBrck,
/*equals sign '='*/                                   Letter.fWordDelim|Letter.URL_CHAR,
/*greater-than sign '>'*/                             Letter.fWordDelim|Letter.ClsBrck,
/*question mark '?'*/                                 Letter.fWordDelim|Letter.URL_CHAR,
/*commercial at '@'*/                                 Letter.fWordDelim|Letter.URL_CHAR,
/*latin capital letter a 'A'*/                        Letter.GerUpper|Letter.EngUpper|Letter.LatinVowel,
/*latin capital letter b 'B'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter c 'C'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter d 'D'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter e 'E'*/                        Letter.GerUpper|Letter.EngUpper|Letter.LatinVowel,
/*latin capital letter f 'F'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter g 'G'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter h 'H'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter i 'I'*/                        Letter.GerUpper|Letter.EngUpper|Letter.UpRomDigits|Letter.LatinVowel,
/*latin capital letter j 'J'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter k 'K'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter l 'L'*/                        Letter.GerUpper|Letter.EngUpper|Letter.UpRomDigits,
/*latin capital letter m 'M'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter n 'N'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter o 'O'*/                        Letter.GerUpper|Letter.EngUpper|Letter.LatinVowel,
/*latin capital letter p 'P'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter q 'Q'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter r 'R'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter s 'S'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter t 'T'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter u 'U'*/                        Letter.GerUpper|Letter.EngUpper|Letter.LatinVowel,
/*latin capital letter v 'V'*/                        Letter.GerUpper|Letter.EngUpper|Letter.UpRomDigits,
/*latin capital letter w 'W'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter x 'X'*/                        Letter.GerUpper|Letter.EngUpper|Letter.UpRomDigits,
/*latin capital letter y 'Y'*/                        Letter.GerUpper|Letter.EngUpper,
/*latin capital letter z 'Z'*/                        Letter.GerUpper|Letter.EngUpper,
/*left square bracket '['*/                           Letter.fWordDelim|Letter.OpnBrck,
/*reverse solidus '\'*/                               Letter.fWordDelim,
/*right square bracket ']'*/                          Letter.fWordDelim|Letter.ClsBrck,
/*circumflex accent '^'*/                             Letter.fWordDelim,
/*low line '_'*/                                      Letter.fWordDelim,
/*grave accent '`'*/                                  Letter.fWordDelim,
/*latin small letter a 'a'*/                          Letter.GerLower|Letter.EngLower|Letter.LatinVowel|Letter.URL_CHAR,
/*latin small letter b 'b'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter c 'c'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter d 'd'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter e 'e'*/                          Letter.GerLower|Letter.EngLower|Letter.LatinVowel|Letter.URL_CHAR,
/*latin small letter f 'f'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter g 'g'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter h 'h'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter i 'i'*/                          Letter.GerLower|Letter.EngLower|Letter.LwRomDigits|Letter.LatinVowel|Letter.URL_CHAR,
/*latin small letter j 'j'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter k 'k'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter l 'l'*/                          Letter.GerLower|Letter.EngLower|Letter.LwRomDigits|Letter.URL_CHAR,
/*latin small letter m 'm'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter n 'n'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter o 'o'*/                          Letter.GerLower|Letter.EngLower|Letter.LatinVowel|Letter.URL_CHAR,
/*latin small letter p 'p'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter q 'q'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter r 'r'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter s 's'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter t 't'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter u 'u'*/                          Letter.GerLower|Letter.EngLower|Letter.LatinVowel|Letter.URL_CHAR,
/*latin small letter v 'v'*/                          Letter.GerLower|Letter.EngLower|Letter.LwRomDigits|Letter.URL_CHAR,
/*latin small letter w 'w'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter x 'x'*/                          Letter.GerLower|Letter.EngLower|Letter.LwRomDigits|Letter.URL_CHAR,
/*latin small letter y 'y'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*latin small letter z 'z'*/                          Letter.GerLower|Letter.EngLower|Letter.URL_CHAR,
/*left curly bracket '{'*/                            Letter.fWordDelim|Letter.OpnBrck,
/*vertical line '|'*/                                 Letter.fWordDelim,
/*right curly bracket '}'*/                           Letter.fWordDelim|Letter.ClsBrck,
/*tilde '~'*/                                         Letter.fWordDelim,
/*delete ''*/                                         Letter.None,
/*padding character (pad) '_'*/                       Letter.fWordDelim,
/*high octet preset (hop) '_'*/                       Letter.None,
/*break permitted here (bph) '''*/                    Letter.None,
/*no break here (nbh) '_'*/                           Letter.fWordDelim,
/*index (ind) '"'*/                                   Letter.None,
/*next line (nel) ':'*/                               Letter.fWordDelim,
/*start of selected area (ssa) '+'*/                  Letter.fWordDelim,
/*end of selected area (esa) '+'*/                    Letter.fWordDelim,
/*character tabulation std::set (hts) '_'*/           Letter.fWordDelim,
/*character tabulation with justification (htj) '%'*/ Letter.fWordDelim,
/*line tabulation std::set (vts) '_'*/                Letter.None,
/*partial line forward (pld) '<'*/                    Letter.fWordDelim,
/*partial line backward (plu) '_'*/                   Letter.fWordDelim,
/*reverse line feed (ri) '_'*/                        Letter.fWordDelim,
/*single-shift two (ss2) '_'*/                        Letter.fWordDelim,
/*single-shift three (ss3) '_'*/                      Letter.fWordDelim,
/*device control std::string (dcs) '_'*/              Letter.fWordDelim,
/*private use one (pu1) '''*/                         Letter.fWordDelim,
/*private use two (pu2) '''*/                         Letter.fWordDelim,
/*std::set transmit state (sts) '"'*/                 Letter.fWordDelim,
/*cancel character (cch) '"'*/                        Letter.fWordDelim,
/*message waiting (mw) ''*/                          Letter.fWordDelim,
/*start of guarded area (spa) '-'*/                   Letter.fWordDelim,
/*end of guarded area (epa) '-'*/                     Letter.fWordDelim,
/*start of std::string (sos) '_'*/                    Letter.fWordDelim,
/*single graphic character introducer (sgci) 'T'*/    Letter.fWordDelim,
/*single character introducer (sci) '_'*/             Letter.fWordDelim,
/*control sequence introducer (csi) '>'*/             Letter.fWordDelim,
/*std::string terminator (st) '_'*/                   Letter.fWordDelim,
/*operating system command (osc) '_'*/                Letter.fWordDelim,
/*privacy message (pm) '_'*/                          Letter.fWordDelim,
/*application program command (apc) '_'*/             Letter.fWordDelim,
/*no-break space '�'*/                                Letter.fWordDelim,
/*inverted exclamation mark '�'*/                     Letter.fWordDelim,
/*cent sign '�'*/                                     Letter.fWordDelim,
/*pound sign '_'*/                                    Letter.fWordDelim,
/*currency sign '�'*/                                 Letter.fWordDelim,
/*yen sign '_'*/                                      Letter.fWordDelim,
/*broken bar '�'*/                                    Letter.fWordDelim,
/*section sign '�'*/                                  Letter.fWordDelim,
/*diaeresis '�'*/                                     Letter.fWordDelim|Letter.RusUpper|Letter.RussianVowel,
/*copyright sign 'c'*/                                Letter.fWordDelim,
/*feminine ordinal indicator '�'*/                    Letter.fWordDelim,
/*left pointing double angle quotation mark '<'*/     Letter.fWordDelim,
/*not sign '�'*/                                      Letter.fWordDelim,
/*soft hyphen '-'*/                                   Letter.fWordDelim,
/*registered sign 'R'*/                               Letter.fWordDelim,
/*macron '�'*/                                        Letter.fWordDelim,
/*degree sign '�'*/                                   Letter.fWordDelim,
/*plus-minus sign '+'*/                               Letter.fWordDelim,
/*superscript two '_'*/                               Letter.fWordDelim,
/*superscript three '_'*/                             Letter.fWordDelim,
/*acute '_'*/                                         Letter.fWordDelim,
/*micro sign '�'*/                                    Letter.fWordDelim|Letter.GerLower|Letter.GerUpper,
/*pilcrow sign '�'*/                                  Letter.fWordDelim,
/*middle dot '�'*/                                    Letter.fWordDelim,
/*cedilla '�'*/                                       Letter.RusLower|Letter.RussianVowel,
/*superscript one '�'*/                               Letter.fWordDelim,
/*masculine ordinal indicator '�'*/                   Letter.fWordDelim,
/*right pointing double angle quotation mark '>'*/    Letter.fWordDelim,
/*vulgar fraction one quarter '_'*/                   Letter.fWordDelim,
/*vulgar fraction one half '_'*/                      Letter.fWordDelim,
/*vulgar fraction three quarters '_'*/                Letter.fWordDelim,
/*inverted question mark '�'*/                        Letter.fWordDelim,
/*latin capital letter a with grave '�'*/             Letter.RusUpper|Letter.RussianVowel,
/*latin capital letter a with acute '�'*/             Letter.RusUpper,
/*latin capital letter a with circumflex '�'*/        Letter.RusUpper|Letter.GerUpper|Letter.EngUpper|Letter.LatinVowel,
/*latin capital letter a with tilde '�'*/             Letter.RusUpper,
/*latin capital letter a with diaeresis '�'*/         Letter.RusUpper|Letter.GerUpper|Letter.LatinVowel,
/*latin capital letter a with ring above '�'*/        Letter.RusUpper|Letter.RussianVowel,
/*latin capital ligature ae '�'*/                     Letter.RusUpper,
/*latin capital letter c with cedilla '�'*/           Letter.RusUpper|Letter.GerUpper|Letter.EngUpper,
/*latin capital letter e with grave '�'*/             Letter.RusUpper|Letter.GerUpper|Letter.EngUpper|Letter.LatinVowel|Letter.RussianVowel,
/*latin capital letter e with acute '�'*/             Letter.RusUpper|Letter.GerUpper|Letter.EngUpper|Letter.LatinVowel,
/*latin capital letter e with circumflex '�'*/        Letter.RusUpper|Letter.GerUpper|Letter.EngUpper|Letter.LatinVowel,
/*latin capital letter e with diaeresis '�'*/         Letter.RusUpper,
/*latin capital letter i with grave '�'*/             Letter.RusUpper,
/*latin capital letter i with acute '�'*/             Letter.RusUpper,
/*latin capital letter i with circumflex '�'*/        Letter.RusUpper|Letter.RussianVowel,
/*latin capital letter i with diaeresis '�'*/         Letter.RusUpper,
/*latin capital letter eth (icelandic) '�'*/          Letter.RusUpper,
/*latin capital letter n with tilde '�'*/             Letter.RusUpper|Letter.GerUpper|Letter.EngUpper,
/*latin capital letter o with grave '�'*/             Letter.RusUpper,
/*latin capital letter o with acute '�'*/             Letter.RusUpper|Letter.RussianVowel,
/*latin capital letter o with circumflex '�'*/        Letter.RusUpper|Letter.GerUpper|Letter.EngUpper|Letter.LatinVowel,
/*latin capital letter o with tilde '�'*/             Letter.RusUpper,
/*latin capital letter o with diaeresis '�'*/         Letter.RusUpper|Letter.GerUpper|Letter.EngUpper|Letter.LatinVowel,
/*multiplication sign '�'*/                           Letter.RusUpper,
/*latin capital letter o with stroke '�'*/            Letter.RusUpper|Letter.UpRomDigits,
/*latin capital letter u with grave '�'*/             Letter.RusUpper,
/*latin capital letter u with acute '�'*/             Letter.RusUpper,
/*latin capital letter u with circumflex '�'*/        Letter.RusUpper|Letter.GerUpper|Letter.EngUpper|Letter.LatinVowel|Letter.RussianVowel,
/*latin capital letter u with diaeresis '�'*/         Letter.RusUpper|Letter.GerUpper|Letter.LatinVowel,
/*latin capital letter y with acute '�'*/             Letter.RusUpper|Letter.RussianVowel,
/*latin capital letter thorn (icelandic) '�'*/        Letter.RusUpper|Letter.RussianVowel,
/*latin small letter sharp s (german) '�'*/           Letter.RusUpper|Letter.GerLower|Letter.GerUpper|Letter.RussianVowel,
/*latin small letter a with grave '�'*/               Letter.RusLower|Letter.RussianVowel,
/*latin small letter a with acute '�'*/               Letter.RusLower,
/*latin small letter a with circumflex '�'*/          Letter.RusLower|Letter.GerLower|Letter.EngLower|Letter.LatinVowel,
/*latin small letter a with tilde '�'*/               Letter.RusLower,
/*latin small letter a with diaeresis '�'*/           Letter.RusLower|Letter.GerLower|Letter.LatinVowel,
/*latin small letter a with ring above '�'*/          Letter.RusLower|Letter.RussianVowel,
/*latin small ligature ae '�'*/                       Letter.RusLower,
/*latin small letter c with cedilla '�'*/             Letter.RusLower|Letter.GerLower|Letter.EngLower,
/*latin small letter e with grave '�'*/               Letter.RusLower|Letter.GerLower|Letter.EngLower|Letter.LatinVowel|Letter.RussianVowel,
/*latin small letter e with acute '�'*/               Letter.RusLower|Letter.GerLower|Letter.EngLower|Letter.LatinVowel,
/*latin small letter e with circumflex '�'*/          Letter.RusLower|Letter.GerLower|Letter.EngLower|Letter.LatinVowel,
/*latin small letter e with diaeresis '�'*/           Letter.RusLower,
/*latin small letter i with grave '�'*/               Letter.RusLower,
/*latin small letter i with acute '�'*/               Letter.RusLower,
/*latin small letter i with circumflex '�'*/          Letter.RusLower|Letter.RussianVowel,
/*latin small letter i with diaeresis '�'*/           Letter.RusLower,
/*latin small letter eth (icelandic) '�'*/            Letter.RusLower,
/*latin small letter n with tilde '�'*/               Letter.RusLower|Letter.GerLower|Letter.EngLower,
/*latin small letter o with grave '�'*/               Letter.RusLower,
/*latin small letter o with acute '�'*/               Letter.RusLower|Letter.RussianVowel,
/*latin small letter o with circumflex '�'*/          Letter.RusLower|Letter.GerLower|Letter.EngLower|Letter.LatinVowel,
/*latin small letter o with tilde '�'*/               Letter.RusLower,
/*latin small letter o with diaeresis '�'*/           Letter.RusLower|Letter.GerLower|Letter.EngLower|Letter.LatinVowel,
/*division sign '�'*/                                 Letter.RusLower,
/*latin small letter o with stroke '�'*/              Letter.RusLower,
/*latin small letter u with grave '�'*/               Letter.RusLower,
/*latin small letter u with acute '�'*/               Letter.RusLower,
/*latin small letter u with circumflex '�'*/          Letter.RusLower|Letter.GerLower|Letter.EngLower|Letter.LatinVowel|Letter.RussianVowel,
/*latin small letter u with diaeresis '�'*/           Letter.RusLower|Letter.GerLower|Letter.LatinVowel,
/*latin small letter y with acute '�'*/               Letter.RusLower|Letter.RussianVowel,
/*latin small letter thorn (icelandic) '�'*/          Letter.RusLower|Letter.RussianVowel,
/*latin small letter y with diaeresis  '�'*/          Letter.RusLower|Letter.RussianVowel
		};

		public static bool IsUpperAlpha(char x, MorphLanguage language) => language switch
		{
			MorphLanguage.Russian => IsRussianUpper(x),
			MorphLanguage.English => IsEnglishUpper(x),
			MorphLanguage.German => IsGermanUpper(x),
			MorphLanguage.Generic => IsGenericUpper(x),
			_ => throw new ArgumentOutOfRangeException(nameof(language), language, "Unexpected language"),
		};

		public static bool IsRussianLower(char x) => ASCII[x].HasFlag(Letter.RusLower);

		public static bool IsRussianUpper(char x) => ASCII[x].HasFlag(Letter.RusUpper);

		public static bool IsRussianAlpha(char x) => IsRussianLower(x) || IsRussianUpper(x);

		public static bool IsEnglishLower(char x) => ASCII[x].HasFlag(Letter.EngLower);

		public static bool IsEnglishUpper(char x) => ASCII[x].HasFlag(Letter.EngUpper);

		public static bool IsEnglishAlpha(char x) => IsEnglishLower(x) || IsEnglishUpper(x);

		public static bool IsGermanLower(char x) => ASCII[x].HasFlag(Letter.GerLower);

		public static bool IsGermanUpper(char x) => ASCII[x].HasFlag(Letter.GerUpper);

		public static bool IsGermanAlpha(char x) => IsGermanLower(x) || IsGermanUpper(x);

		public static bool IsGenericUpper(char x) => ASCII[x].HasFlag(Letter.EngUpper);

		public static bool IsGenericAlpha(char x) => IsEnglishAlpha(x) || x >= 128;

		public static bool IsURLAlpha(char x) => ASCII[x].HasFlag(Letter.URL_CHAR);

		public static bool IsAlpha(char x, MorphLanguage language) => language switch
		{
			MorphLanguage.Russian => IsRussianAlpha(x),
			MorphLanguage.English => IsEnglishAlpha(x),
			MorphLanguage.German => IsGermanAlpha(x),
			MorphLanguage.Generic => IsGenericAlpha(x),
			MorphLanguage.URL => IsURLAlpha(x),
			_ => throw new ArgumentOutOfRangeException(nameof(language), language, "Unexpected language"),
		};

	}
}
