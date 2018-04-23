package iac.rest.webshop.controllers;

import iac.rest.webshop.persistence.Exercise;
import iac.rest.webshop.repositories.ExerciseRepository;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.util.Assert;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletResponse;
import java.util.List;

@RestController
@RequestMapping("/exercises")
public class ExerciseController {

	private ExerciseRepository exerciseRepository;

	public ExerciseController(ExerciseRepository exerciseRepository) {
		this.exerciseRepository = exerciseRepository;
	}

	@PostMapping
	public void addExercise(@RequestBody Exercise exercise) {
		exerciseRepository.save(exercise);
	}

	@GetMapping
	public List<Exercise> getExercises() {
		return exerciseRepository.findAll();
	}

	@GetMapping("/{id}")
	public ResponseEntity<Exercise> getExercise(@PathVariable long id, HttpServletResponse response) {
		// Return 204 when not found
		if (!exerciseRepository.findById(id).isPresent()) return new ResponseEntity<>(HttpStatus.NO_CONTENT);

        Exercise exercise = exerciseRepository.getOne(id);

        return new ResponseEntity<>(exercise, HttpStatus.OK);
	}

	@PutMapping("/{id}")
	public void editExercise(@PathVariable long id, @RequestBody Exercise exercise) {
		Exercise existingExercise = exerciseRepository.getOne(id);

		Assert.notNull(existingExercise, "Exercise not found");
		existingExercise.setName(exercise.getName());
		existingExercise.setDescription(exercise.getDescription());
		existingExercise.setExerciseRecordings(exercise.getExerciseRecordings());
		exerciseRepository.save(existingExercise);
	}

	@DeleteMapping("/{id}")
	public void deleteExercise(@PathVariable long id) {
		exerciseRepository.deleteById(id);
	}
}
