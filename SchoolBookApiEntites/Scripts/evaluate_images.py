import sys
import cv2
import json
import numpy as np
import pytesseract
from skimage.metrics import structural_similarity as ssim
from tensorflow.keras.models import load_model

def preprocess_image(image_path):
    try:
        # Read the image
        image = cv2.imread(image_path)
        gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
        
        # Apply Gaussian blur to remove noise
        blurred = cv2.GaussianBlur(gray, (5, 5), 0)
        
        # Use adaptive thresholding to handle varying lighting conditions
        thresh = cv2.adaptiveThreshold(blurred, 255, cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY_INV, 11, 2)
        
        # Apply morphological operations to clean up the image
        kernel = np.ones((3, 3), np.uint8)
        thresh = cv2.morphologyEx(thresh, cv2.MORPH_CLOSE, kernel)
        
        return image, gray, thresh, None
    except Exception as e:
        return None, None, None, str(e)

def evaluate_image_quality(image, handwriting_ratio):
    try:
        # Quality assessment using SSIM (Structural Similarity Index)
        baseline_image = np.ones(image.shape, dtype=image.dtype) * 255  # assuming a white baseline image for comparison
        quality_score, _ = ssim(image, baseline_image, full=True)
        
        # Adjust quality score based on handwriting ratio
        adjusted_quality_score = quality_score * handwriting_ratio
        
        return adjusted_quality_score
    except Exception as e:
        return str(e)

def detect_handwriting(gray_image, thresh_image):
    try:
        # Detect handwriting using Tesseract
        custom_config = r'--oem 3 --psm 6'
        data = pytesseract.image_to_data(thresh_image, output_type=pytesseract.Output.DICT, config=custom_config)
        
        handwriting_detected = any([word for word in data['text'] if len(word.strip()) > 0])
        
        # Calculate handwriting ratio (number of detected words / total number of words)
        total_words = len(data['text'])
        detected_words = sum(1 for word in data['text'] if len(word.strip()) > 0)
        if detected_words<=10 :
            handwriting_ratio = 4
        elif detected_words<=20 :
            handwriting_ratio = 3
        elif detected_words<=30 :
            handwriting_ratio = 2 
        else :
            handwriting_ratio = 1
        
        return handwriting_detected, handwriting_ratio
    except Exception as e:
        return str(e), 0

def evaluate_image(image_path):
    image, gray, thresh, error = preprocess_image(image_path)
    if error:
        return {"error": f"Error in preprocessing: {error}"}
    
    # Detect handwriting and calculate handwriting ratio
    handwriting_detected, handwriting_ratio = detect_handwriting(gray, thresh)
    if isinstance(handwriting_detected, str):  # error occurred
        return {"error": f"Error in handwriting detection: {handwriting_detected}"}
    
    # Quality assessment adjusted by handwriting ratio
    quality_score = evaluate_image_quality(gray, handwriting_ratio)
    if isinstance(quality_score, str):  # error occurred
        return {"error": f"Error in quality assessment: {quality_score}"}
    
    result = {
        "quality_score": quality_score,
        "handwriting_detected": handwriting_detected,
        "handwriting_ratio": handwriting_ratio
    }
    
    return result

if __name__ == "__main__":
    image_paths = sys.argv[1:]
    results = []
    for image_path in image_paths:
        result = evaluate_image(image_path)
        results.append(result)
    print(json.dumps(results))
